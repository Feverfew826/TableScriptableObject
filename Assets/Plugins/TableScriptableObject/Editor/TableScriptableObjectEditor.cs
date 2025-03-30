using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

// If you use Unity6 or above, then just use https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-uxml-element-MultiColumnListView.html
public class TableScriptableObjectEditor : EditorWindow
{
    private VisualElement _tableParent;
    private MultiColumnListView _multiColumnListView;
    private TableScriptableObject _table;
    private SerializedObject _serializedTable;
    private SerializedProperty _serializedTableRecords;
    private HashSet<IDisposable> _disposables = new(256);

    [MenuItem("Window/TableScriptableObject/TableScriptableObjectEditor", priority = 3021)]
    public static void GetTableScriptableObjectEditor()
    {
        var wnd = GetWindow<TableScriptableObjectEditor>();
        wnd.titleContent = new GUIContent("TableScriptableObjectEditor");
    }

    [MenuItem("Window/TableScriptableObject/New TableScriptableObjectEditor", priority = 3022)]
    public static void GetNewTableScriptableObjectEditor()
    {
        var wnd = CreateWindow<TableScriptableObjectEditor>();
        wnd.titleContent = new GUIContent("TableScriptableObjectEditor");
    }

    public void CreateGUI()
    {
        var objectField = new ObjectField("Table Scriptable Object");
        objectField.objectType = typeof(TableScriptableObject);
        objectField.RegisterValueChangedCallback(OnObjectChanged);
        rootVisualElement.Add(objectField);

        _tableParent = new VisualElement();
        _tableParent.style.left = 0;
        _tableParent.style.right = 0;
        rootVisualElement.Add(_tableParent);
    }

    private void OnDestroy()
    {
        ClearExistingInformation();
        rootVisualElement.Remove(_tableParent);
        _tableParent = null;
    }

    public void ClearExistingInformation()
    {
        if (_multiColumnListView != null)
        {
            _multiColumnListView.itemsSource = null;
            _multiColumnListView.itemsAdded -= TableAdded;

            foreach (var column in _multiColumnListView.columns)
            {
                column.makeCell = null;
                column.bindCell = null;
            }
            _multiColumnListView.columns.Clear();
            _multiColumnListView.Clear();

            _tableParent.Remove(_multiColumnListView);
            _multiColumnListView = null;

            foreach (var disposable in _disposables)
                disposable.Dispose();
            _disposables.Clear();

            _serializedTableRecords = null;
            _serializedTable = null;
            _table = null;
        }
    }

    private void OnObjectChanged(ChangeEvent<UnityEngine.Object> changeEvent)
    {
        if(changeEvent.newValue is not TableScriptableObject table)
            return;

        LoadTable(table);
    }

    private void LoadTable(TableScriptableObject table)
    {
        ClearExistingInformation();

        _table = table;
        // To initialize easily.
        if (_table.TableRecords.Count == 0)
            _table.AddItem();

        _serializedTable = new SerializedObject(_table);
        _serializedTableRecords = _serializedTable.FindProperty("_tableRecords");
        _disposables.Add(_serializedTable);
        _disposables.Add(_serializedTableRecords);

        _multiColumnListView = new MultiColumnListView();
        _multiColumnListView.itemsSource = _table.TableRecords;
        _multiColumnListView.BindProperty(_serializedTableRecords);
        _multiColumnListView.itemsAdded += TableAdded;
        _multiColumnListView.selectionType = SelectionType.Multiple;
        _multiColumnListView.showAddRemoveFooter = true;
        _multiColumnListView.reorderable = true;
        _multiColumnListView.showBorder = true;
        _multiColumnListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        _multiColumnListView.columns.Add(new Column { width = 10 });
        _multiColumnListView.columns[0].makeCell = MakeIndexCell;
        _multiColumnListView.columns[0].bindCell = BindIndexCell;

        for (var i = 0; i < _serializedTableRecords.arraySize; i++)
        {
            using var element = _serializedTableRecords.GetArrayElementAtIndex(i);
            using var enumerator = element.GetNonRecursiveChildEnumerator();
            for(var columnIndex = 0; enumerator.MoveNext(); columnIndex++)
            {
                var property = enumerator.Current;

                if (_multiColumnListView.columns.Contains(property.name) == false)
                {
                    _multiColumnListView.columns.Add(new Column { name = property.name, title = property.displayName, width = 40 + property.displayName.Length * 5});

                    _multiColumnListView.columns[property.name].makeCell = MakePropertyCell;
                    _multiColumnListView.columns[property.name].bindCell = GetBindPropertyCell(property.name);
                }
            }
        }

        _tableParent.Add(_multiColumnListView);
        _multiColumnListView.StretchToParentWidth();
    }

    private VisualElement MakeIndexCell()
    {
        return new Label();
    }

    private VisualElement MakePropertyCell()
    {
        return new PropertyField();
    }

    private void BindIndexCell(VisualElement element, int row)
    {
        (element as Label).text = $" {row.ToString()}";
    }

    private Action<VisualElement, int> GetBindPropertyCell(string propertyName)
    {
        return (VisualElement cellElement, int row) =>
        {
            _serializedTable.Update();
            var property = _serializedTableRecords.GetArrayElementAtIndex(row).FindPropertyRelative(propertyName);
            _disposables.Add(property);

            var propertyField = cellElement as PropertyField;
            propertyField.BindProperty(property);
            propertyField.label = string.Empty;
        };
    }

    private void TableAdded(IEnumerable<int> indices)
    {
        // Prevent null exception.
        foreach (var index in indices)
            _table.InitializedIndexAt(index);
    }
}