using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.IO;

namespace Filesystem.Ntfs
{
        public enum AttType
        {
            Unknown,
            SIA = 0x10,
            AttributeList = 0x20,
            FileName = 0x30,
            ObjectID = 0x40,
            SecurityDescriptor = 0x50,
            VolumeName = 0x60,
            VolumeInformation = 0x70,
            Data = 0x80,
            IndexRoot = 0x90,
            IndexAllocation = 0xA0,
            Bitmap = 0xB0,
            SymbolicLink = 0xC0,
            EA_Information = 0xD0,
            EA = 0xE0,
            LoggedUtilityStream = 0xF0
        }

        internal class MftAttribute
        {
            public MftAttHeader Header { get; }
            public AttType Type => Header.Type;

            public MftAttribute(MftAttHeader header)
            {
                Header = header;
            }
        }

        internal class MftAttHeader
        {
            public AttType Type { get; }

            //public int AttType;
            public int AttLength { get; }
            public short NonResidentFlag { get; }
            public short NameLength { get; }
            public short OffsettotheName { get; }
            public short Flag { get; }
            public short AttID { get; }

            public MftAttHeader(Stream stream)
            {
                //var t = AttType.Unknown;
                //var value = 0x10;
                //if (Enum.IsDefined(typeof(AttType), value))
                //    t = (AttType)value;

                //분석
                byte[] buffer = new byte[16];
                stream.Read(buffer, 0, 4);
                Type = (AttType)buffer[4];

                AttLength = stream.ReadInt32();
                NonResidentFlag = stream.ReadInt16();
                NameLength = stream.ReadInt16();
                OffsettotheName = stream.ReadInt16();
                Flag = stream.ReadInt16();
                AttID = stream.ReadInt16();
            }
        }

        internal class ResidentHeader : MftAttHeader
        {
            public int SizeOfContent { get; }
            public short OffsetOfCount { get; }
            public bool IsIndex { get; }
            public byte Padding { get; }

            public ResidentHeader(Stream stream) : base(stream)
            {
                SizeOfContent = stream.ReadInt32();
                OffsetOfCount = stream.ReadInt16();
                //IsIndex = buffer[0] == 1;
                IsIndex = stream.ReadBool();
                Padding = (byte)stream.ReadByte(); 
            }
        }

        internal class NonResidentHeader : MftAttHeader
        {
            public long StartVcn { get; }
            public long EndVcn { get; }
            public short RunlistOffset { get; }
            public short CompressUnitSize { get; }
            public int Padding { get; }
            public long ContentAllocSize { get; }
            public long ContentActureSize { get; }
            public long ContentInitSize { get; }
            public List<ClusterRun> ClusterRuns { get; } //

            public NonResidentHeader(Stream stream) : base(stream)
            {
                
                StartVcn = stream.ReadInt64();

                EndVcn = stream.ReadInt64();

                RunlistOffset = stream.ReadInt16();

                CompressUnitSize = stream.ReadInt16();
                Padding = stream.ReadInt32();

                ContentAllocSize = stream.ReadInt64();

                ContentActureSize = stream.ReadInt64();

                ContentInitSize = stream.ReadInt64();

                ClusterRuns.Add(new ClusterRun(stream));
            }
        }

        internal class ClusterRun
        {
            public byte ClusterrunHeader { get; }
            public byte[] RunLength { get; }
            public byte[] RunOffset { get; }

            public ClusterRun(Stream stream)
            {
                ClusterrunHeader = (byte)stream.ReadByte();

                int a;
                int b;
                a = ClusterrunHeader >> 4;
                b = ClusterrunHeader & 0x0F;

                int ClusterRunData = a + b;//

                byte[] buffer = new byte[ClusterRunData];
                stream.Read(buffer, 0, b);
                RunLength = buffer;
                
                buffer = new byte[ClusterRunData];
                stream.Read(buffer, 0, a);
                RunOffset = buffer;
            }

        }
        internal class StandardInformation : MftAttribute
        {
            public long CreationTime { get; }
            public long ModifiedTime { get; }
            public long MFTModifiedTime { get; }
            public long LastAccessedTime { get; }
            public int Flags { get; }
            public int MaximumNumberOfVersions { get; }
            public int VersionNumber { get; }
            public int ClassID { get; }
            public int OwnerID { get; }
            public int SecurityID { get; }
            public long QuotaCharged { get; }
            public long UpdateSequenceNumber { get; }

            public StandardInformation(MftAttHeader header, Stream stream) : base(header)
            {
                //byte[] buffer = new byte[header.AttLength]; //
                stream.Seek(24, SeekOrigin.Begin);

                CreationTime = stream.ReadInt64();
                ModifiedTime = stream.ReadInt64();
                MFTModifiedTime = stream.ReadInt16();
                LastAccessedTime = stream.ReadInt16();
                Flags = stream.ReadInt32();
                MaximumNumberOfVersions = stream.ReadInt32();
                VersionNumber = stream.ReadInt32();
                ClassID = stream.ReadInt32();
                OwnerID = stream.ReadInt32();
                SecurityID = stream.ReadInt32();
                QuotaCharged = stream.ReadInt64();
                UpdateSequenceNumber = stream.ReadInt64();
            }
        }

        internal class FileName : MftAttribute
        {
            public long FileReferenceOfParentDirectory { get; }
            public long CreationTime { get; }
            public long ModifiedTime { get; }
            public long MFTModifiedTime { get; }
            public long LastAccessedTime { get; }
            public long AllocatedSizeOfFile { get; }
            public long RealSizeOfFile { get; }
            public int Flags { get; }
            public int ReparseValue { get; }
            public short LengthOfName { get; }
            public short Namespace { get; }
            public string Name { get; }
            public FileName(MftAttHeader header, Stream stream) : base(header)
            {
                //byte[] buffer = new byte[header.AttLength];
                stream.Seek(24, SeekOrigin.Begin);

                FileReferenceOfParentDirectory = stream.ReadInt64();

                CreationTime = stream.ReadInt64();

                ModifiedTime = stream.ReadInt64();

                MFTModifiedTime = stream.ReadInt64();

                LastAccessedTime = stream.ReadInt64();

                AllocatedSizeOfFile = stream.ReadInt64();

                RealSizeOfFile = stream.ReadInt64();
                Flags = stream.ReadInt32();

                ReparseValue = stream.ReadInt32();

                LengthOfName = stream.ReadInt16();

                Namespace = stream.ReadInt16();

                Name = stream.ReadString(LengthOfName);
            }
        }


        internal class Bitmap : MftAttribute
        {
            public byte[] BitFiled { get; }

            public Bitmap(MftAttHeader header, Stream stream) : base(header)
            {
                BitFiled = stream.ReadBytes(header.AttLength);
            }
        }




        internal class DataAttribute : MftAttribute
        {
            //public List<ClusterRun> ClusterRuns { get; }

            public byte[] Data { get; }

            public DataAttribute(MftAttHeader header, Stream stream) : base(header)
            {
                if (header.NonResidentFlag == 1)
                    return;

                // Resident 이면 Data에 값 입력
                var regidentHeader = (ResidentHeader)header;
                Data = new byte[regidentHeader.SizeOfContent];
                stream.Read(Data, 0, Data.Length);
            }
        }
    }

