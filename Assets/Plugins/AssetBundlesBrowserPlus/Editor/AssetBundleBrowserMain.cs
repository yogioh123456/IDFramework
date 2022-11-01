﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
    public class AssetBundleBrowserMain : EditorWindow, IHasCustomMenu, ISerializationCallbackReceiver
    {

        private static AssetBundleBrowserMain s_instance = null;
        internal static AssetBundleBrowserMain instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = GetWindow<AssetBundleBrowserMain>();
                return s_instance;
            }
        }

        internal const float kButtonWidth = 150;

        enum Mode
        {
            Browser,
            Builder,
            Inspect,
        }
        [SerializeField]
        Mode m_Mode;

        [SerializeField]
        int m_DataSourceIndex;
        internal const string KEY_DATASOURCE_INDEX = "KEY_DATASOURCE_INDEX";

        [SerializeField]
        internal AssetBundleManageTab m_ManageTab;

        [SerializeField]
        internal AssetBundleBuildTab m_BuildTab;

        [SerializeField]
        internal AssetBundleInspectTab m_InspectTab;

        private Texture2D m_RefreshTexture;

        const float k_ToolbarPadding = 15;
        const float k_MenubarPadding = 32;

        [MenuItem("Window/AssetBundle Browser Plus", priority = 2050)]
        static void ShowWindow()
        {
            s_instance = null;
            instance.titleContent = new GUIContent("AssetBundlesPlus");
            instance.Show();
        }

        [SerializeField]
        internal bool multiDataSource = false;
        List<AssetBundleDataSource.ABDataSource> m_DataSourceList = null;
        internal int DataSourceCount { get { return m_DataSourceList.Count; } }

        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            if (menu != null)
                menu.AddItem(new GUIContent("Custom Sources"), multiDataSource, FlipDataSource);
        }
        internal void FlipDataSource()
        {
            multiDataSource = !multiDataSource;
        }

        private void OnEnable()
        {
            this._InitSerializedProperties();

            Rect subPos = GetSubWindowArea();
            if (m_ManageTab == null)
                m_ManageTab = new AssetBundleManageTab();
            m_ManageTab.OnEnable(subPos, this);
            if (m_BuildTab == null)
                m_BuildTab = new AssetBundleBuildTab();
            m_BuildTab.OnEnable(this);
            if (m_InspectTab == null)
                m_InspectTab = new AssetBundleInspectTab();
            m_InspectTab.OnEnable(subPos);

            m_RefreshTexture = EditorGUIUtility.FindTexture("Refresh");

            InitDataSources();
        }
        private void InitDataSources()
        {
            //determine if we are "multi source" or not...
            //multiDataSource = false;
            m_DataSourceList = new List<AssetBundleDataSource.ABDataSource>();

            foreach (var info in AssetBundleDataSource.ABDataSourceProviderUtility.CustomABDataSourceTypes)
            {
                m_DataSourceList.AddRange(info.GetMethod("CreateDataSources").Invoke(null, null) as List<AssetBundleDataSource.ABDataSource>);
            }

            multiDataSource = true;
            if (m_DataSourceList.Count > 0)
            {
                m_DataSourceIndex = EditorPrefs.GetInt(KEY_DATASOURCE_INDEX, 0);
                if (m_DataSourceIndex >= m_DataSourceList.Count) m_DataSourceIndex = 0;
                AssetBundleModel.Model.DataSource = m_DataSourceList[m_DataSourceIndex];
                if (_autoRefresh) ReloadBuildMapDataSource();
                this._LoadBuildMaps();
            }
            else AssetBundleModel.Model.DataSource = null;
        }
        private void OnDisable()
        {
            if (m_BuildTab != null)
                m_BuildTab.OnDisable();
            if (m_InspectTab != null)
                m_InspectTab.OnDisable();
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
        }

        private Rect GetSubWindowArea()
        {
            float padding = k_MenubarPadding;
            if (multiDataSource)
                padding += k_MenubarPadding * 0.5f;
            Rect subPos = new Rect(0, padding, position.width, position.height - padding);
            return subPos;
        }

        private void Update()
        {
            switch (m_Mode)
            {
                case Mode.Builder:
                    break;
                case Mode.Inspect:
                    break;
                case Mode.Browser:
                default:
                    m_ManageTab.Update();
                    break;
            }
        }

        private void OnGUI()
        {
            ModeToggle();

            switch (m_Mode)
            {
                case Mode.Builder:
                    // build tab
                    m_BuildTab.OnGUI();
                    // build maps section (Extension)
                    if (typeof(BundleBuildMap).IsInstanceOfType(AssetBundleModel.Model.DataSource))
                    {
                        if ((AssetBundleModel.Model.DataSource as BundleBuildMap).allowCustomBuild) this._DrawBuildMaps();
                    }
                    break;
                case Mode.Inspect:
                    m_InspectTab.OnGUI(GetSubWindowArea());
                    break;
                case Mode.Browser:
                default:
                    m_ManageTab.OnGUI(GetSubWindowArea());
                    break;
            }
        }

        void ModeToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(k_ToolbarPadding);
            bool clicked = false;
            switch (m_Mode)
            {
                case Mode.Browser:
                    clicked = GUILayout.Button(m_RefreshTexture);
                    if (clicked)
                    {
                        ReloadBuildMapDataSource();
                        m_ManageTab.ForceReloadData();
                    }
                    break;
                case Mode.Builder:
                    GUILayout.Space(m_RefreshTexture.width + k_ToolbarPadding);
                    break;
                case Mode.Inspect:
                    clicked = GUILayout.Button(m_RefreshTexture);
                    if (clicked)
                        m_InspectTab.RefreshBundles();
                    break;
            }

            float toolbarWidth = position.width - k_ToolbarPadding * 4 - m_RefreshTexture.width;
            //string[] labels = new string[2] { "Configure", "Build"};
            string[] labels = new string[3] { "Configure", "Build", "Inspect" };
            m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, labels, "LargeButton", GUILayout.Width(toolbarWidth));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (multiDataSource)
            {
                //GUILayout.BeginArea(r);
                GUILayout.BeginHorizontal();

                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("Bundle Data Source:");
                    GUILayout.FlexibleSpace();
                    var c = new GUIContent(string.Format("{0} ({1})", AssetBundleModel.Model.DataSource.Name, AssetBundleModel.Model.DataSource.ProviderName), "Select Asset Bundle Set");
                    if (m_DataSourceList.Count == 0) c = new GUIContent("None", "Browse to Load BuildMaps");
                    if (GUILayout.Button(c, EditorStyles.toolbarPopup))
                    {
                        GenericMenu menu = new GenericMenu();

                        for (int index = 0; index < m_DataSourceList.Count; index++)
                        {
                            var ds = m_DataSourceList[index];
                            if (ds == null)
                                continue;

                            if (index > 0)
                                menu.AddSeparator("");

                            var counter = index;
                            menu.AddItem(new GUIContent(@$"{ds.Name} ({ds.ProviderName})"), false,
                                () =>
                                {
                                    m_DataSourceIndex = counter;
                                    EditorPrefs.SetInt(KEY_DATASOURCE_INDEX, m_DataSourceIndex);
                                    var thisDataSource = ds;
                                    AssetBundleModel.Model.DataSource = thisDataSource;
                                    if (_autoRefresh) ReloadBuildMapDataSource();
                                    m_ManageTab.ForceReloadData();
                                }
                            );
                        }

                        menu.ShowAsContext();
                    }

                    GUILayout.FlexibleSpace();
                    if (AssetBundleModel.Model.DataSource.IsReadOnly())
                    {
                        GUIStyle tbLabel = new GUIStyle(EditorStyles.toolbar);
                        tbLabel.alignment = TextAnchor.MiddleRight;

                        GUILayout.Label("Read Only", tbLabel);
                    }

                    /* Auto Reload Toggle */
                    _autoRefresh = GUILayout.Toggle(
                        _autoRefresh,
                        new GUIContent
                        (
                            "Auto Refresh",
                            "If checked will auto reload data source on open and switch other data source also will auto reload. (Not Recommend, when you have many assets will take a long time to load. *Recommend press refresh button manually.)"
                        )
                    );
                    EditorPrefs.SetBool(KEY_AUTO_REFRESH, _autoRefresh);

                    /* Open folder to load DataSources for BundleBuildMap */
                    Color bc = GUI.backgroundColor;
                    GUI.backgroundColor = new Color32(0, 255, 235, 255);
                    if (GUILayout.Button("Browse Folder to Load BuildMaps", GUILayout.MaxWidth(225f)))
                    {
                        BundleBuildMap.OpenDataSources();
                        InitDataSources();
                    }
                    GUI.backgroundColor = bc;
                }

                GUILayout.EndHorizontal();
                //GUILayout.EndArea();
            }
        }

        #region Extension paramters
        [SerializeField]
        internal List<BundleBuildMap> _buildMaps = new List<BundleBuildMap>();
        [SerializeField]
        internal Vector2 _buildMapSectionScrollPosition;
        internal SerializedObject _soThis;
        internal SerializedProperty _spBuildMaps;

        [SerializeField]
        internal bool _autoRefresh = false;
        internal const string KEY_AUTO_REFRESH = "KEY_AUTO_REFRESH";
        #endregion

        /// <summary>
        /// Extension method for init serialized properties
        /// </summary>
        private void _InitSerializedProperties()
        {
            _soThis = new SerializedObject(this);
            _spBuildMaps = _soThis.FindProperty("_buildMaps");
            _autoRefresh = EditorPrefs.GetBool(KEY_AUTO_REFRESH, false);
        }

        /// <summary>
        /// Extension method for custom build maps
        /// </summary>
        private void _DrawBuildMaps()
        {
            GUILayout.FlexibleSpace();

            // desc label
            var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(new GUIContent("Custom build maps\n(Please check to make sure <AssetNames = AssetPaths> are correct for following build maps)"), centeredStyle);
            EditorGUILayout.Space();

            // list build mpas
            _buildMapSectionScrollPosition = EditorGUILayout.BeginScrollView(_buildMapSectionScrollPosition);
            GUIStyle style = new GUIStyle();
            var bg = new Texture2D(1, 1);
            Color[] pixels = Enumerable.Repeat(new Color(0f, 0.35f, 0.32f, 0.5f), Screen.width * Screen.height).ToArray();
            bg.SetPixels(pixels);
            bg.Apply();
            style.normal.background = bg;
            GUILayout.BeginVertical(style);
            Color bc = GUI.backgroundColor;
            GUI.backgroundColor = new Color32(0, 255, 235, 255);
            _soThis.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_spBuildMaps, true);
            _soThis.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck()) this._SaveBuildMaps();
            GUI.backgroundColor = bc;

            // reload button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bc = GUI.backgroundColor;
            GUI.backgroundColor = new Color32(229, 149, 255, 255);
            if (GUILayout.Button("Reload from current data source", GUILayout.MaxWidth(220f))) this._LoadBuildMaps();
            GUI.backgroundColor = bc;
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void _LoadBuildMaps()
        {
            if (typeof(BundleBuildMap).IsInstanceOfType(AssetBundleModel.Model.DataSource))
            {
                this._buildMaps = (AssetBundleModel.Model.DataSource as BundleBuildMap).customBuildMaps;
            }
        }

        private void _SaveBuildMaps()
        {
            if (typeof(BundleBuildMap).IsInstanceOfType(AssetBundleModel.Model.DataSource))
            {
                (AssetBundleModel.Model.DataSource as BundleBuildMap).customBuildMaps = this._buildMaps;
                (AssetBundleModel.Model.DataSource as BundleBuildMap).Save();
            }
        }

        public static void ReloadBuildMapDataSource()
        {
            if (typeof(BundleBuildMap).IsInstanceOfType(AssetBundleModel.Model.DataSource))
            {
                (AssetBundleModel.Model.DataSource as BundleBuildMap).RefreshAllAssetBundle();
            }
        }
    }
}
