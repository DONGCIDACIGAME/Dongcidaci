# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: HeroCfg.proto
"""Generated protocol buffer code."""
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='HeroCfg.proto',
  package='DongciDaci',
  syntax='proto3',
  serialized_options=b'\n!com.trinitigames.server.conf.auto',
  create_key=_descriptor._internal_create_key,
  serialized_pb=b'\n\rHeroCfg.proto\x12\nDongciDaci\"\x8a\x01\n\x0bHeroBaseCfg\x12\n\n\x02ID\x18\x01 \x01(\r\x12\x0c\n\x04Name\x18\x02 \x01(\t\x12\x0e\n\x06Prefab\x18\x03 \x01(\t\x12\r\n\x05Speed\x18\x04 \x01(\x02\x12\x14\n\x0c\x44\x61shDistance\x18\x05 \x01(\x02\x12\x14\n\x0c\x41ttackRadius\x18\x06 \x01(\x02\x12\x16\n\x0eInteractRadius\x18\x07 \x01(\x02\"\xab\x01\n\x0cHeroCfg_Data\x12I\n\x11HeroBaseCfg_items\x18\x01 \x03(\x0b\x32..DongciDaci.HeroCfg_Data.HeroBaseCfgItemsEntry\x1aP\n\x15HeroBaseCfgItemsEntry\x12\x0b\n\x03key\x18\x01 \x01(\r\x12&\n\x05value\x18\x02 \x01(\x0b\x32\x17.DongciDaci.HeroBaseCfg:\x02\x38\x01\x42#\n!com.trinitigames.server.conf.autob\x06proto3'
)




_HEROBASECFG = _descriptor.Descriptor(
  name='HeroBaseCfg',
  full_name='DongciDaci.HeroBaseCfg',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='ID', full_name='DongciDaci.HeroBaseCfg.ID', index=0,
      number=1, type=13, cpp_type=3, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Name', full_name='DongciDaci.HeroBaseCfg.Name', index=1,
      number=2, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Prefab', full_name='DongciDaci.HeroBaseCfg.Prefab', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Speed', full_name='DongciDaci.HeroBaseCfg.Speed', index=3,
      number=4, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='DashDistance', full_name='DongciDaci.HeroBaseCfg.DashDistance', index=4,
      number=5, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='AttackRadius', full_name='DongciDaci.HeroBaseCfg.AttackRadius', index=5,
      number=6, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='InteractRadius', full_name='DongciDaci.HeroBaseCfg.InteractRadius', index=6,
      number=7, type=2, cpp_type=6, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=30,
  serialized_end=168,
)


_HEROCFG_DATA_HEROBASECFGITEMSENTRY = _descriptor.Descriptor(
  name='HeroBaseCfgItemsEntry',
  full_name='DongciDaci.HeroCfg_Data.HeroBaseCfgItemsEntry',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='key', full_name='DongciDaci.HeroCfg_Data.HeroBaseCfgItemsEntry.key', index=0,
      number=1, type=13, cpp_type=3, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='value', full_name='DongciDaci.HeroCfg_Data.HeroBaseCfgItemsEntry.value', index=1,
      number=2, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=b'8\001',
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=262,
  serialized_end=342,
)

_HEROCFG_DATA = _descriptor.Descriptor(
  name='HeroCfg_Data',
  full_name='DongciDaci.HeroCfg_Data',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='HeroBaseCfg_items', full_name='DongciDaci.HeroCfg_Data.HeroBaseCfg_items', index=0,
      number=1, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[_HEROCFG_DATA_HEROBASECFGITEMSENTRY, ],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=171,
  serialized_end=342,
)

_HEROCFG_DATA_HEROBASECFGITEMSENTRY.fields_by_name['value'].message_type = _HEROBASECFG
_HEROCFG_DATA_HEROBASECFGITEMSENTRY.containing_type = _HEROCFG_DATA
_HEROCFG_DATA.fields_by_name['HeroBaseCfg_items'].message_type = _HEROCFG_DATA_HEROBASECFGITEMSENTRY
DESCRIPTOR.message_types_by_name['HeroBaseCfg'] = _HEROBASECFG
DESCRIPTOR.message_types_by_name['HeroCfg_Data'] = _HEROCFG_DATA
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

HeroBaseCfg = _reflection.GeneratedProtocolMessageType('HeroBaseCfg', (_message.Message,), {
  'DESCRIPTOR' : _HEROBASECFG,
  '__module__' : 'HeroCfg_pb2'
  # @@protoc_insertion_point(class_scope:DongciDaci.HeroBaseCfg)
  })
_sym_db.RegisterMessage(HeroBaseCfg)

HeroCfg_Data = _reflection.GeneratedProtocolMessageType('HeroCfg_Data', (_message.Message,), {

  'HeroBaseCfgItemsEntry' : _reflection.GeneratedProtocolMessageType('HeroBaseCfgItemsEntry', (_message.Message,), {
    'DESCRIPTOR' : _HEROCFG_DATA_HEROBASECFGITEMSENTRY,
    '__module__' : 'HeroCfg_pb2'
    # @@protoc_insertion_point(class_scope:DongciDaci.HeroCfg_Data.HeroBaseCfgItemsEntry)
    })
  ,
  'DESCRIPTOR' : _HEROCFG_DATA,
  '__module__' : 'HeroCfg_pb2'
  # @@protoc_insertion_point(class_scope:DongciDaci.HeroCfg_Data)
  })
_sym_db.RegisterMessage(HeroCfg_Data)
_sym_db.RegisterMessage(HeroCfg_Data.HeroBaseCfgItemsEntry)


DESCRIPTOR._options = None
_HEROCFG_DATA_HEROBASECFGITEMSENTRY._options = None
# @@protoc_insertion_point(module_scope)
