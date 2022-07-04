using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TipMaterialEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(HairStyleEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(CharacterTypeGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumBaseDebuffDictionary))]
public class MySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
