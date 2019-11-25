using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileServer.Models;
using Microsoft.EntityFrameworkCore;
using FileServer.DomainEntity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text;

namespace FileServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DbContext context = null;
        public HomeController(ILogger<HomeController> logger, FileContext _context)
        {
            context = _context;
            _logger = logger;
        }


        public async Task<IActionResult> UploadFile([FromForm]IFormCollection collection)
        {

            if (collection.Files.Any())
            {
                foreach (var formFile in collection.Files)
                {
                    var filedata = new FileData();
                    filedata.IP = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4() + ":" + Request.HttpContext.Connection.LocalPort.ToString();
                    filedata.Id = Guid.NewGuid();
                    filedata.FileName = collection.Files.FirstOrDefault().FileName;
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filesavepath = path + formFile.FileName;
                    using (var stream = new FileStream(filesavepath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    filedata.FilePath = filesavepath;
                    filedata.CreateTime = DateTime.Now;
                    context.Set<FileData>().Add(filedata);
                }
                await context.SaveChangesAsync();
            }
            return new RedirectResult("Index");
        }


        public async Task<IActionResult> Index()
        {
            var data = await context.Set<FileData>().OrderByDescending(t => t.CreateTime).ToListAsync();
            return View(data);
        }




        public async Task Download(Guid Id)
        {
            var tem = context.Set<FileData>().FirstOrDefault(t => t.Id == Id);
            string url = "http://" + tem.IP + "/Home/DownloadB?Id=" + tem.Id;
            _logger.LogDebug(url);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET"; 
            using (var myResponse = (HttpWebResponse)myRequest.GetResponse())
            {
                using (Stream stream = (myResponse.GetResponseStream()))
                {
                    var ms = StreamToMemoryStream(stream);
                    ms.Seek(0, SeekOrigin.Begin); int buffsize = (int)ms.Length; //rs.Length 此流不支持查找,先转为MemoryStream
                    byte[] bytes = new byte[buffsize];
                    ms.Read(bytes, 0, buffsize);
                    Response.ContentType = "application/octet-stream";
                    HttpContext.Response.Headers["Content-Type"] = "application/x-bittorrent";
                    HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=" + tem.FileName + "";
                    await HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    await HttpContext.Response.Body.FlushAsync();
                }
            } 
        }
        MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }




        public async Task DownloadB(Guid Id)
        {
            var tem = context.Set<FileData>().FirstOrDefault(t => t.Id == Id);
            HttpContext.Response.Body.Close();
            Response.ContentType = "application/octet-stream";
            HttpContext.Response.Headers["Content-Type"] = "application/x-bittorrent";
            HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=" + tem.FileName + "";
            FileStream t = new FileStream(tem.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            HttpContext.Response.Headers["ContentLength"] = t.Length.ToString();
            try
            {
                byte[] bytes = new byte[t.Length];
                HttpContext.Response.Headers.ContentLength = bytes.Length;
                t.Read(bytes, 0, bytes.Length);
                await HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                await HttpContext.Response.Body.FlushAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {

                t.Close();
                t.Dispose();
            }

        }

        private Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            var connection = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTIONSTRING");
            return View(new ErrorViewModel { Connection = connection });
        }
    }
}
