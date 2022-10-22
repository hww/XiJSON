/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

namespace XiJSON.Interfaces
{
    public interface IArchive
    {
        bool IsWriting { get; }
        bool IsReading { get; }
        void Write(object jso, string path);
        void Read(object jso, string path);
    }
}