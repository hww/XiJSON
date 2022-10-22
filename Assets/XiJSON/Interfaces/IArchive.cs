/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

namespace XiJSON.Interfaces
{
    public interface IArchive
    {
        string Path { get; }
        bool IsWriting { get; }
        bool IsReading { get; }
        void Write(object jso);
        void Read(object jso);
    }
}