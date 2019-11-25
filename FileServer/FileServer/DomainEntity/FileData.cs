using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.DomainEntity
{

    public class FileData
    {
        public Guid Id { get; set; }
    

        /// <summary>
        /// 服务器名字
        /// </summary>
        public string IP { get; set; }
        
        /// <summary>
        /// 服务器路径
        /// </summary>
        public string FilePath { get; set; }


        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } 
    }
}
