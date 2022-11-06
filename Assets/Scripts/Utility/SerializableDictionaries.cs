using System;
using UnityEngine;
using UnityEditor;
using TotemEnums;

[Serializable]
public class SpearMaterialEnumGameObjectDictionary : SerializableDictionary<SpearMaterial, GameObject>
{ }

[Serializable]
public class HairStyleEnumGameObjectDictionary : SerializableDictionary<HairStyleEnum, GameObject>
{ }

[Serializable]
public class CharacterTypeGameObjectDictionary : SerializableDictionary<CharacterType, GameObject>
{ }

[Serializable]
public class ElementEnumGameObjectDictionary : SerializableDictionary<ElementEnum, GameObject>
{ }

[Serializable]
public class ElementEnumBaseDebuffDictionary : SerializableDictionary<ElementEnum, BaseDebuff>
{ }

[Serializable]
public class ElementEnumSceneIdDictionary : SerializableDictionary<ElementEnum, int>
{ }

[Serializable]
public class SoundTypeEnumAudioClipInfoDictionary : SerializableDictionary<SoundType, AudioClipInfo>
{ }