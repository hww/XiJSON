/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

namespace XiJSON.Interfaces
{
    public interface IArchive
    {
        bool IsWriting { get; }
        bool IsReading { get; }
        bool Write(object jso, string path);
        bool Read(object jso, string path);
    }
}