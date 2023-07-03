// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: HeroCfg.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace DongciDaci {

  /// <summary>Holder for reflection information generated from HeroCfg.proto</summary>
  public static partial class HeroCfgReflection {

    #region Descriptor
    /// <summary>File descriptor for HeroCfg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HeroCfgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1IZXJvQ2ZnLnByb3RvEgpEb25nY2lEYWNpIooBCgtIZXJvQmFzZUNmZxIK",
            "CgJJRBgBIAEoDRIMCgROYW1lGAIgASgJEg4KBlByZWZhYhgDIAEoCRINCgVT",
            "cGVlZBgEIAEoAhIUCgxEYXNoRGlzdGFuY2UYBSABKAISFAoMQXR0YWNrUmFk",
            "aXVzGAYgASgCEhYKDkludGVyYWN0UmFkaXVzGAcgASgCIqsBCgxIZXJvQ2Zn",
            "X0RhdGESSQoRSGVyb0Jhc2VDZmdfaXRlbXMYASADKAsyLi5Eb25nY2lEYWNp",
            "Lkhlcm9DZmdfRGF0YS5IZXJvQmFzZUNmZ0l0ZW1zRW50cnkaUAoVSGVyb0Jh",
            "c2VDZmdJdGVtc0VudHJ5EgsKA2tleRgBIAEoDRImCgV2YWx1ZRgCIAEoCzIX",
            "LkRvbmdjaURhY2kuSGVyb0Jhc2VDZmc6AjgBQiMKIWNvbS50cmluaXRpZ2Ft",
            "ZXMuc2VydmVyLmNvbmYuYXV0b2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::DongciDaci.HeroBaseCfg), global::DongciDaci.HeroBaseCfg.Parser, new[]{ "ID", "Name", "Prefab", "Speed", "DashDistance", "AttackRadius", "InteractRadius" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::DongciDaci.HeroCfg_Data), global::DongciDaci.HeroCfg_Data.Parser, new[]{ "HeroBaseCfgItems" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class HeroBaseCfg : pb::IMessage<HeroBaseCfg>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HeroBaseCfg> _parser = new pb::MessageParser<HeroBaseCfg>(() => new HeroBaseCfg());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<HeroBaseCfg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::DongciDaci.HeroCfgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroBaseCfg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroBaseCfg(HeroBaseCfg other) : this() {
      iD_ = other.iD_;
      name_ = other.name_;
      prefab_ = other.prefab_;
      speed_ = other.speed_;
      dashDistance_ = other.dashDistance_;
      attackRadius_ = other.attackRadius_;
      interactRadius_ = other.interactRadius_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroBaseCfg Clone() {
      return new HeroBaseCfg(this);
    }

    /// <summary>Field number for the "ID" field.</summary>
    public const int IDFieldNumber = 1;
    private uint iD_;
    /// <summary>
    ///* ID 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint ID {
      get { return iD_; }
      set {
        iD_ = value;
      }
    }

    /// <summary>Field number for the "Name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    /// <summary>
    ///* 名称 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Prefab" field.</summary>
    public const int PrefabFieldNumber = 3;
    private string prefab_ = "";
    /// <summary>
    ///* 预制体路径 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Prefab {
      get { return prefab_; }
      set {
        prefab_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Speed" field.</summary>
    public const int SpeedFieldNumber = 4;
    private float speed_;
    /// <summary>
    ///* 角色的移动速度 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float Speed {
      get { return speed_; }
      set {
        speed_ = value;
      }
    }

    /// <summary>Field number for the "DashDistance" field.</summary>
    public const int DashDistanceFieldNumber = 5;
    private float dashDistance_;
    /// <summary>
    ///* 角色的冲刺距离 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float DashDistance {
      get { return dashDistance_; }
      set {
        dashDistance_ = value;
      }
    }

    /// <summary>Field number for the "AttackRadius" field.</summary>
    public const int AttackRadiusFieldNumber = 6;
    private float attackRadius_;
    /// <summary>
    ///* 攻击半径 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float AttackRadius {
      get { return attackRadius_; }
      set {
        attackRadius_ = value;
      }
    }

    /// <summary>Field number for the "InteractRadius" field.</summary>
    public const int InteractRadiusFieldNumber = 7;
    private float interactRadius_;
    /// <summary>
    ///* 交互半径 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public float InteractRadius {
      get { return interactRadius_; }
      set {
        interactRadius_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as HeroBaseCfg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(HeroBaseCfg other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ID != other.ID) return false;
      if (Name != other.Name) return false;
      if (Prefab != other.Prefab) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DashDistance, other.DashDistance)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(AttackRadius, other.AttackRadius)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(InteractRadius, other.InteractRadius)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ID != 0) hash ^= ID.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Prefab.Length != 0) hash ^= Prefab.GetHashCode();
      if (Speed != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
      if (DashDistance != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DashDistance);
      if (AttackRadius != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(AttackRadius);
      if (InteractRadius != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(InteractRadius);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ID != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(ID);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Prefab.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Prefab);
      }
      if (Speed != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Speed);
      }
      if (DashDistance != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(DashDistance);
      }
      if (AttackRadius != 0F) {
        output.WriteRawTag(53);
        output.WriteFloat(AttackRadius);
      }
      if (InteractRadius != 0F) {
        output.WriteRawTag(61);
        output.WriteFloat(InteractRadius);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ID != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(ID);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Prefab.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Prefab);
      }
      if (Speed != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Speed);
      }
      if (DashDistance != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(DashDistance);
      }
      if (AttackRadius != 0F) {
        output.WriteRawTag(53);
        output.WriteFloat(AttackRadius);
      }
      if (InteractRadius != 0F) {
        output.WriteRawTag(61);
        output.WriteFloat(InteractRadius);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (ID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(ID);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Prefab.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Prefab);
      }
      if (Speed != 0F) {
        size += 1 + 4;
      }
      if (DashDistance != 0F) {
        size += 1 + 4;
      }
      if (AttackRadius != 0F) {
        size += 1 + 4;
      }
      if (InteractRadius != 0F) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(HeroBaseCfg other) {
      if (other == null) {
        return;
      }
      if (other.ID != 0) {
        ID = other.ID;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Prefab.Length != 0) {
        Prefab = other.Prefab;
      }
      if (other.Speed != 0F) {
        Speed = other.Speed;
      }
      if (other.DashDistance != 0F) {
        DashDistance = other.DashDistance;
      }
      if (other.AttackRadius != 0F) {
        AttackRadius = other.AttackRadius;
      }
      if (other.InteractRadius != 0F) {
        InteractRadius = other.InteractRadius;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ID = input.ReadUInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 26: {
            Prefab = input.ReadString();
            break;
          }
          case 37: {
            Speed = input.ReadFloat();
            break;
          }
          case 45: {
            DashDistance = input.ReadFloat();
            break;
          }
          case 53: {
            AttackRadius = input.ReadFloat();
            break;
          }
          case 61: {
            InteractRadius = input.ReadFloat();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ID = input.ReadUInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 26: {
            Prefab = input.ReadString();
            break;
          }
          case 37: {
            Speed = input.ReadFloat();
            break;
          }
          case 45: {
            DashDistance = input.ReadFloat();
            break;
          }
          case 53: {
            AttackRadius = input.ReadFloat();
            break;
          }
          case 61: {
            InteractRadius = input.ReadFloat();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class HeroCfg_Data : pb::IMessage<HeroCfg_Data>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HeroCfg_Data> _parser = new pb::MessageParser<HeroCfg_Data>(() => new HeroCfg_Data());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<HeroCfg_Data> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::DongciDaci.HeroCfgReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroCfg_Data() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroCfg_Data(HeroCfg_Data other) : this() {
      heroBaseCfgItems_ = other.heroBaseCfgItems_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroCfg_Data Clone() {
      return new HeroCfg_Data(this);
    }

    /// <summary>Field number for the "HeroBaseCfg_items" field.</summary>
    public const int HeroBaseCfgItemsFieldNumber = 1;
    private static readonly pbc::MapField<uint, global::DongciDaci.HeroBaseCfg>.Codec _map_heroBaseCfgItems_codec
        = new pbc::MapField<uint, global::DongciDaci.HeroBaseCfg>.Codec(pb::FieldCodec.ForUInt32(8, 0), pb::FieldCodec.ForMessage(18, global::DongciDaci.HeroBaseCfg.Parser), 10);
    private readonly pbc::MapField<uint, global::DongciDaci.HeroBaseCfg> heroBaseCfgItems_ = new pbc::MapField<uint, global::DongciDaci.HeroBaseCfg>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::MapField<uint, global::DongciDaci.HeroBaseCfg> HeroBaseCfgItems {
      get { return heroBaseCfgItems_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as HeroCfg_Data);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(HeroCfg_Data other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!HeroBaseCfgItems.Equals(other.HeroBaseCfgItems)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= HeroBaseCfgItems.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      heroBaseCfgItems_.WriteTo(output, _map_heroBaseCfgItems_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      heroBaseCfgItems_.WriteTo(ref output, _map_heroBaseCfgItems_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += heroBaseCfgItems_.CalculateSize(_map_heroBaseCfgItems_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(HeroCfg_Data other) {
      if (other == null) {
        return;
      }
      heroBaseCfgItems_.Add(other.heroBaseCfgItems_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            heroBaseCfgItems_.AddEntriesFrom(input, _map_heroBaseCfgItems_codec);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            heroBaseCfgItems_.AddEntriesFrom(ref input, _map_heroBaseCfgItems_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
