using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpearMaterialEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(HairStyleEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(CharacterTypeGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumGameObjectDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumBaseDebuffDictionary))]
[CustomPropertyDrawer(typeof(ElementEnumSceneIdDictionary))]
[CustomPropertyDrawer(typeof(SoundTypeEnumAudioClipInfoDictionary))]
public class MySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
