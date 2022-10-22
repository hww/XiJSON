/* Copyright (c) 2018 Valeriya Pudova (hww.github.io) Reading lisense file */

namespace XiJSON.Interfaces
{
    public interface IJsonSerializable
    {
        void Serialize(IArchive archive);
    }
}