/***
 *░░░░███████╗░██████╗██╗░░██╗░██████╗░██╗░░░██╗██████╗░░██████╗░░░
 *░░░░██╔════╝██╔════╝██║░░██║██╔═══██╗██║░░░██║██╔══██╗██╔════╝░░░
 *░░░░█████╗░░██║░░░░░███████║██║░░░██║██║░░░██║██████╔╝██║░░░░░░░░
 *░░░░██╔══╝░░██║░░░░░██╔══██║██║░░░██║╚██╗░██╔╝██╔══██╗██║░░░░░░░░
 *░░░░███████╗╚██████╗██║░░██║╚██████╔╝░╚████╔╝░██║░░██║╚██████╗░░░
 *░░░░╚══════╝░╚═════╝╚═╝░░╚═╝░╚═════╝░░░╚═══╝░░╚═╝░░╚═╝░╚═════╝░░░
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;


public class SkinnedMeshBoneCheckerEditor : Editor
{
    // Добавление нового пункта в контекстное меню GameObject.
    [MenuItem("GameObject/Check Skinned Mesh Bones", false, 10)]
    static void CheckSkinnedMeshBonesInChildren(MenuCommand menuCommand)
    {
        // Получение выбранного объекта в иерархии.
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject != null)
        {
            // Проверяем, есть ли SkinnedMeshRenderer у самого выбранного объекта.
            SkinnedMeshRenderer skinnedMesh = selectedGameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMesh != null)
            {
                CheckBones(skinnedMesh);
            }

            // Проверка костей у объекта и всех его потомков.
            CheckBonesInAllChildren(selectedGameObject.transform);
        }
        else
        {
            // Вывод сообщения в консоль, если объект не выбран.
            Debug.Log("No object selected in the hierarchy.");
        }
    }

    // Рекурсивная функция для проверки всех потомков переданного transform.
    static void CheckBonesInAllChildren(Transform parent)
    {
        // Перебор всех потомков текущего transform.
        foreach (Transform child in parent)
        {
            // Попытка получить компонент SkinnedMeshRenderer у потомка.
            SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                // Проверка костей у найденного SkinnedMeshRenderer.
                CheckBones(skinnedMeshRenderer);
            }
            // Если у потомка есть свои потомки, рекурсивно вызываем проверку на них.
            if (child.childCount > 0)
            {
                CheckBonesInAllChildren(child);
            }
        }
    }

    // Функция для проверки костей у SkinnedMeshRenderer.
    static void CheckBones(SkinnedMeshRenderer skinnedMesh)
    {
        // Перебор всех костей в SkinnedMeshRenderer.
        foreach (Transform bone in skinnedMesh.bones)
        {
            // Проверка на null каждой кости.
            if (bone == null)
            {
                // Вывод ошибки в консоль, если обнаружена отсутствующая кость.
                Debug.LogError("Missing bone detected in '" + skinnedMesh.gameObject.name + "'");
                return;
            }
        }
        // Вывод сообщения об успешной проверке, если все кости на месте.
        Debug.Log("All bones are intact in '" + skinnedMesh.gameObject.name + "'");
    }
}


public class SkeletonFixer : MonoBehaviour
{
    // Пункт меню для исправления костей в SkinnedMeshRenderer
    [MenuItem("GameObject/Fix Skinned Mesh Bones in Children", false, 10)]
    static void FixSkinnedMeshBonesInChildren(MenuCommand menuCommand)
    {
        // Получаем выбранный объект
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null) return;

        // Применяем исправление к каждому SkinnedMeshRenderer в дочерних объектах
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in selectedObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            RemoveBrokenBones(skinnedMeshRenderer);
        }

        // Выводим сообщение о завершении операции
        Debug.Log("Skinned Mesh Bones fixing process is completed.");
    }

    // Функция для удаления поврежденных костей и замены их на пустышки
    static void RemoveBrokenBones(SkinnedMeshRenderer skinnedMeshRenderer)
    {
        // Проверяем наличие костей и корневой кости
        if (skinnedMeshRenderer.bones == null || skinnedMeshRenderer.rootBone == null) return;

        Transform[] bones = skinnedMeshRenderer.bones;
        bool hasMissingBones = false;
        for (int i = 0; i < bones.Length; i++)
        {
            // Проверяем отсутствие кости
            if (bones[i] == null)
            {
                hasMissingBones = true;
                // Создаем новую кость-заглушку
                GameObject fakeBone = new GameObject($"fake_{skinnedMeshRenderer.gameObject.name}_{i}");
                // Родитель для заглушки - корневая кость
                fakeBone.transform.SetParent(skinnedMeshRenderer.rootBone);
                // Позиционируем заглушку
                fakeBone.transform.localPosition = Vector3.zero;
                fakeBone.transform.localRotation = Quaternion.identity;
                // Заменяем отсутствующую кость на заглушку
                bones[i] = fakeBone.transform;
            }
        }

        // Обновляем массив костей, если были найдены и заменены отсутствующие кости
        if (hasMissingBones)
        {
            skinnedMeshRenderer.bones = bones;
        }
    }
}
