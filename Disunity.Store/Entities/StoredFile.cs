using System;


namespace Disunity.Store.Entities {

    public class StoredFile {

        public Guid Guid { get; set; }

        public uint ObjectId { get; set; }

        public string FileName { get; set; }

        public string FileInfo { get; set; }

    }

}