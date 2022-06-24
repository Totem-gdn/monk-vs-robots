using enums;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class TipMaterialEnumGameObjectDictionary : SerializableDictionary<TipMaterialEnum, GameObject>
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