﻿using Neo.IO;
using System;
using System.IO;
using NLog;

namespace Neo.Network.Payloads
{
    internal class InvPayload : ISerializable
    {
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public InventoryType Type;
        public UInt256[] Hashes;

        public int Size => sizeof(InventoryType) + Hashes.GetVarSize();

        public static InvPayload Create(InventoryType type, params UInt256[] hashes)
        {
            return new InvPayload
            {
                Type = type,
                Hashes = hashes
            };
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            Type = (InventoryType)reader.ReadByte();
            if (!Enum.IsDefined(typeof(InventoryType), Type))
                throw new FormatException();
            Hashes = reader.ReadSerializableArray<UInt256>();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Hashes);
		}
    }
}
