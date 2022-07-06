using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TipMaterialEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(HairStyleEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(CharacterTypeGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumBaseDebuffDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumSceneIdDictionary))]
public class MySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
