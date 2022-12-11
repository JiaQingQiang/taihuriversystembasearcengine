using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace taihu
{
    public partial class Form1 : Form
    {
        private System.Drawing.Point mouse_offset;
        string title = "";
        DateTimePicker pDTP;
        DateTimePicker pDTP2;
        ComboBox combo;
        string SearchName = "";
        int a = 1;
        int b = 1;
        DataGridView dg;
        string Name;
        DataTable daT;
        int flag = 0;//默认左键漫游
        int draw = 0;//默认未选择查看水资源
        int h = 0;
        public Form1()
        {
            InitializeComponent();
        }
        //
        /// <summary>
        /// //属性查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;

            this.axTOCControl1.SetBuddyControl(this.axMapControl1);
            mTOCControl = this.axTOCControl1.Object as ITOCControl;

            //行政查询加载行政区

            IMap pMap = axMapControl1.Map;
            comboBox1.Items.Clear();

            //按照图层名获取图层
            int index = 0;
            index = getlayer_id("县市所在地");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;
            //提取数据
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            IFeatureCursor pfur = featureclass.Search(null, true);
            int nameindex = featureclass.FindField("RNAME");
            IFeature pfeature = pfur.NextFeature();
            while (pfeature != null)
            {
                string name = pfeature.get_Value(nameindex).ToString();
                comboBox3.Items.Add(name);
                pfeature = pfur.NextFeature();
            }

            //按水文分区加载名称
            //查询主要流域
            int index1 = 0;
            index1 = getlayer_id("流域主要水域");
            ILayer pLayer1 = axMapControl1.get_Layer(index1);
            pLayer1.Visible = true;
            //提取数据
            IFeatureLayer featurelayer1 = pLayer1 as IFeatureLayer;
            IFeatureClass featureclass1 = featurelayer1.FeatureClass;
            int nameindex1 = featureclass1.FindField("NAME");
            IFeatureCursor pfur1 = featureclass1.Search(null, true);
            IFeature pfeature1 = pfur1.NextFeature();
            while (pfeature1 != null)
            {
                string name1 = pfeature1.get_Value(nameindex1).ToString();
                if (name1 != " ")
                {
                    if (name1 != null)
                    {
                        comboBox4.Items.Add(name1);
                    }
                }
                pfeature1 = pfur1.NextFeature();
            }
            //鹰眼固定
            axMapControl2.AutoMouseWheel = false;
            for (int i = 0; i < 12; i++)
            //将太湖水资源地图的前8个次要要素图层隐藏，显示第9层以后的
            {
                if (i < 9) axMapControl2.get_Layer(i).Visible = false;
                if (i > 9) axMapControl2.get_Layer(i).Visible = true;
            }
            chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
            b = 2;
            UpdateTable();
        }


       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            //查询雨量站,在名称查询里下拉显示雨量站名称
            comboBox1.Items.Clear();
            //按照图层名获取图层
            int index = 0;
            index = getlayer_id("雨量站");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;

            index = getlayer_id("水文站");
            ILayer pLayer1 = axMapControl1.get_Layer(index);
            pLayer1.Visible = false;

            index = getlayer_id("水质监测站点");
            ILayer pLayer2 = axMapControl1.get_Layer(index);
            pLayer2.Visible = false;
            axMapControl1.Refresh();
            //提出数据         
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            IFeatureCursor pfur = featureclass.Search(null, true);
            int nameindex = featureclass.FindField("STNM");
            IFeature pfeature = pfur.NextFeature();
            while (pfeature != null)
            {
                string name = pfeature.get_Value(nameindex).ToString();
                comboBox1.Items.Add(name);
                pfeature = pfur.NextFeature();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            //查询雨量站,在名称查询里下拉显示雨量站名称
            comboBox1.Items.Clear();
            //按照图层名获取图层
            int index = 0;
            index = getlayer_id("水文站");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;

            index = getlayer_id("雨量站");
            ILayer pLayer1 = axMapControl1.get_Layer(index);
            pLayer1.Visible = false;

            index = getlayer_id("水质监测站点");
            ILayer pLayer2 = axMapControl1.get_Layer(index);
            pLayer2.Visible = false;
            axMapControl1.Refresh();
            //提出数据
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            IFeatureCursor pfur = featureclass.Search(null, true);
            int nameindex = featureclass.FindField("STNM");
            IFeature pfeature = pfur.NextFeature();
            while (pfeature != null)
            {
                string name = pfeature.get_Value(nameindex).ToString();
                comboBox1.Items.Add(name);
                pfeature = pfur.NextFeature();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

            //查询雨量站,在名称查询里下拉显示雨量站名称
            comboBox1.Items.Clear();
            //按照图层名获取图层
            int index = 0;
            index = getlayer_id("水质监测站点");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;

            index = getlayer_id("雨量站");
            ILayer pLayer1 = axMapControl1.get_Layer(index);
            pLayer1.Visible = false;

            index = getlayer_id("水文站");
            ILayer pLayer2 = axMapControl1.get_Layer(index);
            pLayer2.Visible = false;
            axMapControl1.Refresh();
            //提取站点名的数据
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            IFeatureCursor pfur = featureclass.Search(null, true);
            int nameindex = featureclass.FindField("测站名称");
            IFeature pfeature = pfur.NextFeature();
            while (pfeature != null)
            {
                string name = pfeature.get_Value(nameindex).ToString();
                comboBox1.Items.Add(name);
                pfeature = pfur.NextFeature();
            }

            //提取水质的数据，将水质类别放入下拉框中
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Ⅰ");
            comboBox2.Items.Add("Ⅰ-Ⅱ");
            comboBox2.Items.Add("Ⅱ");
            comboBox2.Items.Add("Ⅱ-Ⅲ");
            comboBox2.Items.Add("Ⅱ～Ⅲ");
            comboBox2.Items.Add("Ⅲ");
            comboBox2.Items.Add("Ⅲ-Ⅳ");
            comboBox2.Items.Add("Ⅲ～Ⅳ");
            comboBox2.Items.Add("Ⅳ");
            comboBox2.Items.Add("Ⅴ");

        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //在地图上高亮显示，并在下方表格里显示其属性
            //选择雨量站
            if (radioButton1.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的 
                int index = 0;
                index = getlayer_id("雨量站");
                IFeatureLayer featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;
                search_by_name(featurelayer);
            }
            //选择水文站
            if (radioButton2.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的   
                int index = 0;
                index = getlayer_id("水文站");
                IFeatureLayer featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层           
                search_by_name(featurelayer);
            }
            //选择水质资源站
            if (radioButton3.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的   
                int index = 0;
                index = getlayer_id("水质监测站点");
                IFeatureLayer featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层           
                search_by_name(featurelayer);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //按照水质查找
            axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的   
            int layerindex = 0;
            layerindex = getlayer_id("水质监测站点");
            IFeatureLayer featurelayer = axMapControl1.get_Layer(layerindex) as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;

            //按照水质类别查找
            //属性表列名
            dataGridView1.Rows.Clear();
            IFields pFields = featurelayer.FeatureClass.Fields;
            dataGridView1.ColumnCount = pFields.FieldCount;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fldName = pFields.get_Field(i).Name;
                dataGridView1.Columns[i].Name = fldName;
                dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            //查找
            axMapControl1.Map.ClearSelection();
            int index = comboBox1.SelectedIndex;
            string level = comboBox2.Text;
            ISpatialFilter PSF = new SpatialFilterClass();

            PSF.WhereClause = "水质类别 = '" + level + "'";
            IFeatureCursor pfur = featureclass.Search(PSF, true);
            IFeature pfeature = pfur.NextFeature();
            while (pfeature != null)
            {
                axMapControl1.Map.SelectFeature(featurelayer, pfeature);//高亮显示
                //加字段
                string[] fldValue = new string[pFields.FieldCount];
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName;
                    fldName = pFields.get_Field(i).Name;
                    if (fldName == featurelayer.FeatureClass.ShapeFieldName)
                    {
                        fldValue[i] = Convert.ToString(pfeature.Shape.GeometryType);
                    }
                    else
                        fldValue[i] = Convert.ToString(pfeature.get_Value(i));
                }
                dataGridView1.Rows.Add(fldValue);
                pfeature = pfur.NextFeature();
            }
            //局部刷新
            axMapControl1.Extent = axMapControl1.FullExtent;

        }


        public int getlayer_id(String layername)//通过图层名获取图层
        {
            int index = 0;
            IActiveView pActiveView = axMapControl1.ActiveView;
            IMap pmap = pActiveView.FocusMap;
            for (int i = 0; i < pmap.LayerCount; i++)
            {
                if (pmap.get_Layer(i).Name == layername)
                    index = i;
            }
            return index;
        }


        public void search_by_name(IFeatureLayer featurelayer)//通过站点名称选取要素，并显示属性
        {
            IFeatureClass featureclass = featurelayer.FeatureClass;
            int index = comboBox1.SelectedIndex;
            IFeature pFeature = featureclass.GetFeature(index + 1);
            axMapControl1.Map.SelectFeature(featurelayer, pFeature);

            //目标窗口居中且放大
            IGeometry pGeo = pFeature.Shape;
            IEnvelope pEnv = pGeo.Envelope;
            if (pEnv.Height < 0.1)
                pEnv.Expand(0.2, 0.2, false);
            else
                pEnv.Expand(1.2, 1.2, true);
            (axMapControl1.Map as IActiveView).Extent = pEnv;
            axMapControl1.Refresh();

            //局部刷新地图，使得其不闪退
            IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
            viewRefresh.ProgressiveDrawing = true;
            viewRefresh.RefreshItem(featurelayer);
            //显示属性去属性框
            dataGridView1.Rows.Clear();
            IFields pFields;
            pFields = featurelayer.FeatureClass.Fields;
            dataGridView1.ColumnCount = pFields.FieldCount;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fldName = pFields.get_Field(i).Name;
                dataGridView1.Columns[i].Name = fldName;
                dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            string[] fldValue = new string[pFields.FieldCount];
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fldName;
                fldName = pFields.get_Field(i).Name;
                if (fldName == featurelayer.FeatureClass.ShapeFieldName)
                {
                    fldValue[i] = Convert.ToString(pFeature.Shape.GeometryType);
                }
                else
                    fldValue[i] = Convert.ToString(pFeature.get_Value(i));
            }
            dataGridView1.Rows.Add(fldValue);
            //高亮之后扩大到选中的地方
            //axMapControl1.Extent = pf2.Shape.Envelope;

        }


        public static string ParseFieldType(esriFieldType fieldType)//在添加属性时将EsriType 转换为String 
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }

       
        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        { 
            //设置图层
            IFeatureLayer featurelayer = axMapControl1.get_Layer(0) as IFeatureLayer;//默认为第一个图层
            IFeatureLayer featurelayer1 = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureLayer featurelayer2 = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureLayer featurelayer3 = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureLayer featurelayer4 = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureLayer featurelayer5 = axMapControl1.get_Layer(0) as IFeatureLayer;
            axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的 
            int index = 0;
            index = getlayer_id("雨量站");
            featurelayer1 = axMapControl1.get_Layer(index) as IFeatureLayer;
            index = getlayer_id("水文站");
            featurelayer2 = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层           
            index = getlayer_id("水质监测站点");
            featurelayer3 = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层 
            index = getlayer_id("流域主要河流_注记");
            featurelayer4 = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层 
            featurelayer4.Selectable = false;
            index = getlayer_id("太湖流域高程图2");
            featurelayer5 = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层 
            featurelayer5.Selectable = false;

            if (radioButton1.Checked == true)
            {
                featurelayer = featurelayer1;
                featurelayer1.Selectable = true;
                featurelayer2.Selectable = false;
                featurelayer3.Selectable = false;
            }
            //选择水文站
            if (radioButton2.Checked == true)
            {
                featurelayer = featurelayer2;
                featurelayer1.Selectable = false;
                featurelayer2.Selectable = true;
                featurelayer3.Selectable = false;
            }
            if (radioButton3.Checked == true)
            {
                featurelayer = featurelayer3;
                featurelayer1.Selectable = false;
                featurelayer2.Selectable = false;
                featurelayer3.Selectable = true;
            }

////////////当点击图层且点击了过程线绘制
            if (draw == 1)
            {
                index = getlayer_id("重要站点");
                featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层 
              
            }

            if (e.button == 1 && draw == 1&&h==1)
            {
                IGeometry selectGeometry = null;
                IEnvelope pEnv;
                IActiveView pActiveView = axMapControl1.ActiveView;
                IMap pMap = axMapControl1.Map;

                pEnv = axMapControl1.TrackRectangle();
                if (pEnv.IsEmpty == true)//true说明画了点，开始框选，否则是点选
                {
                    ESRI.ArcGIS.esriSystem.tagRECT r;
                    r.bottom = e.y + 5;
                    r.top = e.y - 5;
                    r.left = e.x - 5;
                    r.right = e.x + 5;
                    pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);
                    pEnv.SpatialReference = pActiveView.FocusMap.SpatialReference;
                }
                selectGeometry = pEnv as IGeometry;
                axMapControl1.Map.SelectByShape(selectGeometry, null, false);

                //建属性表
                dataGridView1.Rows.Clear();
                IFields pFields;
                pFields = featurelayer.FeatureClass.Fields;
                dataGridView1.ColumnCount = pFields.FieldCount;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName = pFields.get_Field(i).Name;
                    dataGridView1.Columns[i].Name = fldName;
                    dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
                }
                //获取选中的要素
                IFeatureSelection pFS = featurelayer as IFeatureSelection;
                ISelectionSet pSS = pFS.SelectionSet;
                ICursor pCur;
                pSS.Search(null, true, out pCur);

                //在属性表插入属性
                IFeatureCursor pFCur = pCur as IFeatureCursor;
                IFeature pFea = pFCur.NextFeature();
                Name = null;
                while (pFea != null)
                {
                    Name = Convert.ToString(pFea.get_Value(4));
                    axMapControl1.Map.SelectFeature(featurelayer, pFea);//高亮显示                                                                        
                    string[] fldValue = new string[pFields.FieldCount];
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        string fldName;
                        fldName = pFields.get_Field(i).Name;
                        if (fldName == featurelayer.FeatureClass.ShapeFieldName)
                        {
                            fldValue[i] = Convert.ToString(pFea.Shape.GeometryType);
                        }
                        else
                            fldValue[i] = Convert.ToString(pFea.get_Value(i));
                    }
                    dataGridView1.Rows.Add(fldValue);
                    pFea = pFCur.NextFeature();
                }
                axMapControl1.Refresh();
                if (Name != null)
                {
                    if (Name == "望亭(太)" || Name == "大浦口" || Name == "洞庭西山" || Name == "夹浦" || Name == "小梅口")
                    {
                        a = 1;//对应太湖五站平均水位表
                        this.panel1.Controls.Clear();
                        chart1.Titles.Clear();
                        chart1.Series.Clear();
                        this.panel1.Visible = false;
                        CreateContainer();//创建过程水位线展示框
                    }
                    else if (Name == "太浦匣(上)_" || Name == "望亭(太)" || Name == "常熟")
                    {
                        a = 2;//对应引排水量表
                        this.panel1.Controls.Clear();
                        chart1.Titles.Clear();
                        chart1.Series.Clear();
                        this.panel1.Visible = false;
                        CreateContainer();//创建过程水位线展示框
                    }
                    else if (Name == "常州" || Name == "无锡" || Name == "杭长桥" || Name == "甘露" || Name == "苏州" ||
                        Name == "嘉兴" || Name == "平望" || Name == "琳桥")
                    {
                        a = 3;//对应重要站点及降雨量表
                        this.panel1.Controls.Clear();
                        chart1.Titles.Clear();
                        chart1.Series.Clear();
                        this.panel1.Visible = false;
                        CreateContainer();//创建过程水位线展示框
                    }
                }
            }



                //flag=0时，默认漫游
                if (e.button == 1 && flag == 0 &&h==0)
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHand;
                axMapControl1.Pan();
            }

            //flag=1时，开启点选和框选功能
            if (e.button == 1 && flag == 1)
            {              
                //判断现在选的是哪个图层
                //选择雨量站

                //点选框选
                IGeometry selectGeometry = null;
                IEnvelope pEnv;
                IActiveView pActiveView = axMapControl1.ActiveView;
                IMap pMap = axMapControl1.Map;

                pEnv = axMapControl1.TrackRectangle();
                if (pEnv.IsEmpty == true)//true说明画了框框，开始框选，否则是点选
                {
                    ESRI.ArcGIS.esriSystem.tagRECT r;
                    r.bottom = e.y + 5;
                    r.top = e.y - 5;
                    r.left = e.x - 5;
                    r.right = e.x + 5;
                    pActiveView.ScreenDisplay.DisplayTransformation.TransformRect(pEnv, ref r, 4);
                    pEnv.SpatialReference = pActiveView.FocusMap.SpatialReference;
                }
                selectGeometry = pEnv as IGeometry;
                axMapControl1.Map.SelectByShape(selectGeometry, null, false);

                //建属性表
                dataGridView1.Rows.Clear();
                IFields pFields;
                pFields = featurelayer.FeatureClass.Fields;
                dataGridView1.ColumnCount = pFields.FieldCount;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName = pFields.get_Field(i).Name;
                    dataGridView1.Columns[i].Name = fldName;
                    dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
                }
                //获取选中的要素
                IFeatureSelection pFS = featurelayer as IFeatureSelection;
                ISelectionSet pSS = pFS.SelectionSet;
                ICursor pCur;
                pSS.Search(null, true, out pCur);

                //在属性表插入属性
                IFeatureCursor pFCur = pCur as IFeatureCursor;
                IFeature pFea = pFCur.NextFeature();
                while (pFea != null)
                {
                    Name = Convert.ToString(pFea.get_Value(4));
                    axMapControl1.Map.SelectFeature(featurelayer, pFea);//高亮显示                                                                        
                    string[] fldValue = new string[pFields.FieldCount];
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        string fldName;
                        fldName = pFields.get_Field(i).Name;
                        if (fldName == featurelayer.FeatureClass.ShapeFieldName)
                        {
                            fldValue[i] = Convert.ToString(pFea.Shape.GeometryType);
                        }
                        else
                            fldValue[i] = Convert.ToString(pFea.get_Value(i));
                    }
                    dataGridView1.Rows.Add(fldValue);
                    pFea = pFCur.NextFeature();
                }
                axMapControl1.Refresh();
          
            }

            //左击，多边形选取
            if (e.button == 1 && flag == 2)
            {

                //画多边形
                IGeometry selectGeometry = null;
                IActiveView pActiveView = axMapControl1.ActiveView;
                IGeometry Polygon = axMapControl1.TrackPolygon();
                if (Polygon.IsEmpty == true)
                {
                    IPolygonElement PolygonElement = new PolygonElementClass();
                    IElement pElement = PolygonElement as IElement;
                    //设置多边形属性（内部透明）
                    IRgbColor pColor = new RgbColorClass();
                    ILineSymbol pOutLine = new SimpleLineSymbolClass();
                    IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                    pColor = new RgbColorClass();
                    pColor.Transparency = 0;
                    pOutLine.Color = pColor;
                    pFillSymbol.Color = pColor;
                    pFillSymbol.Outline = pOutLine;
                    IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
                    IGraphicsContainer pGraphicsContainer = axMapControl1.Map as IGraphicsContainer;
                    pFillShapeElement.Symbol = pFillSymbol;
                    pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);

                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }

                selectGeometry = Polygon as IGeometry;
                axMapControl1.Map.SelectByShape(selectGeometry, null, false);
                axMapControl1.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                IFeatureClass pFC = featurelayer.FeatureClass;

                dataGridView1.Rows.Clear();
                IFields pFields;
                pFields = featurelayer.FeatureClass.Fields;
                dataGridView1.ColumnCount = pFields.FieldCount;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName = pFields.get_Field(i).Name;
                    dataGridView1.Columns[i].Name = fldName;
                    dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
                }

                IFeatureSelection pFS = featurelayer as IFeatureSelection;
                ISelectionSet pSS = pFS.SelectionSet;
                ICursor pCur;
                pSS.Search(null, true, out pCur);

                IFeatureCursor pFCur = pCur as IFeatureCursor;
                IFeature pFea = pFCur.NextFeature();
                IFeatureLayer pFealyr = featurelayer as IFeatureLayer;
                pFC = pFealyr.FeatureClass;
                while (pFea != null)
                {
                    axMapControl1.Map.SelectFeature(featurelayer, pFea);//高亮显示                                                                        
                    string[] fldValue = new string[pFields.FieldCount];
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        string fldName;
                        fldName = pFields.get_Field(i).Name;
                        if (fldName == featurelayer.FeatureClass.ShapeFieldName)
                        {
                            fldValue[i] = Convert.ToString(pFea.Shape.GeometryType);
                        }
                        else
                            fldValue[i] = Convert.ToString(pFea.get_Value(i));
                    }
                    dataGridView1.Rows.Add(fldValue);
                    pFea = pFCur.NextFeature();
                }
            }

            //右击显示基本功能              
            if (e.button == 2) //e.button=2代表右击 
            {
                contextMenuStrip1.Show(axMapControl1, new System.Drawing.Point(e.x, e.y));
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(e.mapX, e.mapY);
                //将地图的中心点移动到鼠标点击的点pPoint
                axMapControl1.CenterAt(pPoint);

            }//如果点击的是图层，则弹出右击菜单  

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //点击属性
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            IQueryFilter pQuery = new QueryFilterClass();
            int count = this.dataGridView1.SelectedRows.Count;
            string val;
            string col;
            col = this.dataGridView1.Columns[0].Name;
            //当只选中一行时
            if (count == 1)
            {
                val = this.dataGridView1.SelectedRows[0].Cells[col].Value.ToString();
                //设置高亮要素的查询条件
                pQuery.WhereClause = col + "=" + val;
            }
            else//当选中多行时
            {
                int i;
                string str;
                for (i = 0; i < count - 1; i++)
                {
                    val = this.dataGridView1.SelectedRows[i].Cells[col].Value.ToString();
                    str = col + "=" + val + " OR ";
                    pQuery.WhereClause += str;
                }
                //添加最后一个要素的条件
                val = this.dataGridView1.SelectedRows[i].Cells[col].Value.ToString();
                str = col + "=" + val;
                pQuery.WhereClause += str;
            }

            int index = 0;
            index = getlayer_id("雨量站");
            IFeatureLayer featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;
            if (radioButton1.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的 
                index = getlayer_id("雨量站");
                featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;
            }
            //选择水文站
            if (radioButton2.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的   
                index = getlayer_id("水文站");
                featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层           
            }
            //选择水质资源站
            if (radioButton3.Checked == true)
            {
                axMapControl1.Map.ClearSelection(); //每次变换清空原来的选择的   
                index = getlayer_id("水质监测站点");
                featurelayer = axMapControl1.get_Layer(index) as IFeatureLayer;//找到CITY图层           
            }

            IFeatureSelection pFeatSelection;
            pFeatSelection = featurelayer as IFeatureSelection;
            pFeatSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
        }

        /// <summary>
        /// //GIS基本功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //// //

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //按照行政类别查找
            string Name = comboBox3.Text;

            //按照图层名获取图层
            int index = 0;
            index = getlayer_id("县市所在地");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;

            //提取数据
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            int nameindex = featureclass.FindField("RNAME");
            ISpatialFilter PSF = new SpatialFilterClass();
            PSF.WhereClause = "RNAME = '" + Name + "'";
            IFeatureCursor pfur = featureclass.Search(PSF, true);    //获取字段
            axMapControl1.Map.ClearSelection();
            IFeature pfeature = pfur.NextFeature();
            axMapControl1.Map.SelectFeature(featurelayer, pfeature);

            //目标窗口居中且放大
            IGeometry pGeo = pfeature.Shape;
            IEnvelope pEnv = pGeo.Envelope;
            if (pEnv.Height < 0.1)
                pEnv.Expand(0.3, 0.3, false);
            else
                pEnv.Expand(1.2, 1.2, true);
            (axMapControl1.Map as IActiveView).Extent = pEnv;
            //局部刷新
            axMapControl1.Refresh();
            IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
            viewRefresh.ProgressiveDrawing = true;
            viewRefresh.RefreshItem(featurelayer);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //按水文分区切换
            //按照图层名获取图层
            //将combox4的txt赋给name
            string Name = comboBox4.Text;
            int index = 0;
            index = getlayer_id("流域主要水域");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;
            //提取数据
            IFeatureLayer featurelayer = pLayer as IFeatureLayer;
            IFeatureClass featureclass = featurelayer.FeatureClass;
            int nameindex = featureclass.FindField("NAME");
            ISpatialFilter PSF = new SpatialFilterClass();
            PSF.WhereClause = "NAME = '" + Name + "'";
            IFeatureCursor pfur = featureclass.Search(PSF, true);
            IFeature pfeature = pfur.NextFeature();    //获取字段
            string name = pfeature.get_Value(nameindex).ToString();
            //居中显示 属性表里有一个null 所以要判断去除 否则报错
            if (name != null)
            {
                axMapControl1.Map.ClearSelection();
                axMapControl1.Map.SelectFeature(featurelayer, pfeature);
                IGeometry pGeo = pfeature.Shape;
                IEnvelope pEnv = pGeo.Envelope;
                if (pEnv.Height < 0.1)
                    pEnv.Expand(0.15, 0.15, false);
                else
                    pEnv.Expand(1.2, 1.2, true);
                (axMapControl1.Map as IActiveView).Extent = pEnv;
                axMapControl1.Refresh();
            }
            axMapControl1.Refresh();
            IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
            viewRefresh.ProgressiveDrawing = true;
            viewRefresh.RefreshItem(featurelayer);
        }

        //切换图层顺序
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            if (e.button == 1)
            {
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                mTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    if (layer is IAnnotationSublayer)
                    {
                        return;
                    }
                    else
                    {
                        pMoveLayer = layer;
                    }
                }
            }
        }

        private void 漫游ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = 0;
            h = 0;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHand;
            axMapControl1.Pan();
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(0.5, 0.5, true);   //放大2倍
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();
        }

        private void 拖框放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.axMapControl1.Extent = this.axMapControl1.TrackRectangle();
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(2, 2, true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.ActiveView.Refresh();

        }

        private void 全景显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
        }


        //鹰眼同步
        private bool bCanDrag;              //鹰眼地图上的矩形框可移动的标志
        private IPoint pMoveRectPoint;      //记录在移动鹰眼地图上的矩形框时鼠标的位置
        private IEnvelope pEnv;             //记录数据视图的Extent
        public ISimpleFillSymbol oFillobject;

        private void SynchronizeEagleEye()
        {
            if (axMapControl2.LayerCount > 0)
            {
                axMapControl2.ClearLayers();
            }

            //设置鹰眼和主地图的坐标系统一致
            axMapControl2.SpatialReference = axMapControl1.SpatialReference;
            for (int i = axMapControl1.LayerCount - 1; i >= 0; i--)
            {
                //使鹰眼视图与数据视图的图层上下顺序保持一致
                ILayer pLayer = axMapControl1.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCompositeLayer = (ICompositeLayer)pLayer;
                    for (int j = pCompositeLayer.Count - 1; j >= 0; j--)
                    {
                        ILayer pSubLayer = pCompositeLayer.get_Layer(j);
                        IFeatureLayer pFeatureLayer = pSubLayer as IFeatureLayer;
                        if (pFeatureLayer != null)
                        {
                            //由于鹰眼地图较小，所以过滤点图层不添加
                            if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                                && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                            {
                                axMapControl2.AddLayer(pLayer);
                            }
                        }
                    }
                }

                else
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    if (pFeatureLayer != null)
                    {
                        if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                            && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                        {
                            axMapControl2.AddLayer(pLayer);
                        }
                    }
                }

                //设置鹰眼地图全图显示  
                axMapControl2.Extent = axMapControl1.FullExtent;////////////////////////////////////////
                pEnv = axMapControl1.Extent as IEnvelope;
                DrawRectangle(pEnv);
                axMapControl2.ActiveView.Refresh();
            }

        }

        //编写图形框，以及获取颜色的方法
        private void DrawRectangle(IEnvelope pEnvelope)
        {
            //在绘制前，清除鹰眼中之前绘制的矩形框
            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;
            pGraphicsContainer.DeleteAllElements();
            //得到当前视图范围
            IRectangleElement pRectangleElement = new RectangleElementClass();
            IElement pElement = pRectangleElement as IElement;
            pElement.Geometry = pEnvelope;
            //设置矩形框（实质为中间透明度面）
            IRgbColor pColor = new RgbColorClass();
            pColor = GetRgbColor(255, 0, 0);
            pColor.Transparency = 255;
            ILineSymbol pOutLine = new SimpleLineSymbolClass();
            pOutLine.Width = 1.5;
            pOutLine.Color = pColor;
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pColor = new RgbColorClass();
            pColor.Transparency = 0;
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutLine;
            //向鹰眼中添加矩形框
            IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
            pFillShapeElement.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
            //刷新
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }


        private IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
            {
                return pRgbColor;
            }
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = 255;
            pRgbColor.Green = 0;
            pRgbColor.Blue = 0;
            return pRgbColor;
        }


        private void axMapControl2_OnMouseDown_1(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            IPoint point = new ESRI.ArcGIS.Geometry.Point();
            point.PutCoords(e.mapX, e.mapY);
            axMapControl1.CenterAt(point);
            if (axMapControl2.Map.LayerCount > 0)
            {
                //按下鼠标左键移动矩形框
                if (e.button == 1)
                {
                    //如果指针落在鹰眼的矩形框中，标记可移动
                    if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    {
                        bCanDrag = true;
                    }
                    pMoveRectPoint = new PointClass();
                    pMoveRectPoint.PutCoords(e.mapX, e.mapY);  //记录点击的第一个点的坐标
                }
                //按下鼠标右键绘制矩形框
                else if (e.button == 2)
                {
                    IEnvelope pEnvelope = axMapControl2.TrackRectangle();
                    IPoint pTempPoint = new PointClass();
                    pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
                    axMapControl1.Extent = pEnvelope;
                    //矩形框的高宽和数据试图的高宽不一定成正比，这里做一个中心调整
                    axMapControl1.CenterAt(pTempPoint);
                }
            }
        }

        private void axMapControl2_OnMouseMove_1(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                //如果鼠标移动到矩形框中，鼠标换成小手，表示可以拖动
                axMapControl2.MousePointer = esriControlsMousePointer.esriPointerHand;
                if (e.button == 2)  //如果在内部按下鼠标右键，将鼠标演示设置为默认样式
                {
                    axMapControl2.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }
            }
            else
            {
                //在其他位置将鼠标设为默认的样式
                axMapControl2.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            if (bCanDrag)
            {
                double Dx, Dy;  //记录鼠标移动的距离
                Dx = e.mapX - pMoveRectPoint.X;
                Dy = e.mapY - pMoveRectPoint.Y;
                pEnv.Offset(Dx, Dy); //根据偏移量更改 pEnv 位置
                pMoveRectPoint.PutCoords(e.mapX, e.mapY);
                DrawRectangle(pEnv);
                axMapControl1.Extent = pEnv;
            }
        }

        private void axMapControl2_OnMouseUp_1(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button == 1 && pMoveRectPoint != null)
            {
                if (e.mapX == pMoveRectPoint.X && e.mapY == pMoveRectPoint.Y)
                {
                    axMapControl1.CenterAt(pMoveRectPoint);
                }
                bCanDrag = false;
            }
        }
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
            axMapControl2.Extent = axMapControl2.FullExtent;

        }

        private void axMapControl2_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
        }

        private void axMapControl2_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
        }

        //toc图层移动

        public ITOCControl mTOCControl;
        public ILayer pMoveLayer;//需要被调整的图层；
        public int toIndex;//将要调整到的目标图层的索引；


        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            if (e.button == 1)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                mTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
                IMap pMap = this.axMapControl1.ActiveView.FocusMap;
                if (item == esriTOCControlItem.esriTOCControlItemLayer || layer != null)
                {
                    if (pMoveLayer != null)
                    {
                        ILayer pTempLayer;
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pTempLayer = pMap.get_Layer(i);
                            if (pTempLayer == layer)
                            {
                                toIndex = i;
                            }
                        }
                        pMap.MoveLayer(pMoveLayer, toIndex);
                        axMapControl1.ActiveView.Refresh();
                        mTOCControl.Update();
                    }
                }
            }
        }


     
/// <summary>
/// /水资源信息数据查询
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("日期:{0:yyyy-MM-dd};数值:{1:F3} ",dg.Rows[i].Cells[0].Value.ToString() , dp.YValues[0]);
            }
        }

        private void UpdateTable()
        {
            string connString = "server=47.102.138.168;database=GIS;uid=GISJQQ;pwd=Jqq963741ArcObjects";
            SqlConnection SC = new SqlConnection(connString);
            SC.Open();
            SqlCommand SCmd = new SqlCommand();
            SCmd.Connection = SC;
            SCmd.CommandType = CommandType.Text;
            switch (b)
            {
                case 2:
                    SCmd.CommandText = "select * from ZDSW where date='2017-01-04'";
                    break;
                case 1:
                    if(comboBox6.SelectedIndex == 0)
                    {
                        SCmd.CommandText = "select * from TH5 where Date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'and Station='"+comboBox7.SelectedItem+"'";
                    }
                    else if(comboBox6.SelectedIndex == 1||comboBox6.SelectedIndex==2)
                    {
                        SCmd.CommandText = "select * from ZDSW where date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'and site='"+comboBox7.SelectedItem+"'";
                    }
                    else
                    {
                        SCmd.CommandText = "select * from YPSL_3 where Date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'and Station='" + comboBox7.SelectedItem + "'";
                    }
                    break;
            }
            SqlDataAdapter SDA = new SqlDataAdapter(SCmd);
            DataSet ds = new DataSet();
            SDA.Fill(ds, "table1");
            DataTable dt = ds.Tables["table1"];
            SDA.Dispose();
            SC.Close();
            foreach (DataRow dr in dt.Rows)
            {
                if (b == 3)
                {
                    dataGridView1.Rows.Add(dr["date"], dr["site"], dr["waterlevel"]);
                }
                else if(b==1)
                {
                    if(comboBox6.SelectedIndex==0)
                    {
                        dataGridView2.Rows.Add(dr["Date"], dr["Station"], dr["WaterH"],3);
                    }
                    else if(comboBox6.SelectedIndex==1)
                    {
                        dataGridView2.Rows.Add(dr["date"], dr["site"], dr["waterlevel"],3);
                    }
                    else if(comboBox6.SelectedIndex==2)
                    {
                        dataGridView2.Rows.Add(dr["date"], dr["site"], dr["rainfall"],3);
                    }
                    else
                    {
                        dataGridView2.Rows.Add(dr["Date"], dr["Station"], dr["YPSL"]);
                    }
                }
                else
                {
                    dataGridView2.Rows.Add(dr["date"], dr["site"], dr["waterlevel"], 3);
                }
            }
        }

       

        private void axLicenseControl2_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //点击点选站点
            axMapControl1.Map.ClearSelection();
            axMapControl1.ActiveView.Refresh();
            flag = 1;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void 取消选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.ClearSelection();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            axMapControl1.ActiveView.Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //多边形选取
            axMapControl1.Map.ClearSelection();
            axMapControl1.ActiveView.Refresh();
            flag = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            a = 1;
            this.panel1.Controls.Clear();
            chart1.Titles.Clear();
            chart1.Series.Clear();
            CreateContainer();
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Search();
            if (combo.SelectedItem.ToString() == "太浦口")
            {
                SearchName = "大浦口";
            }
            else if (combo.SelectedItem.ToString() == "望亭太")
            {
                SearchName = "望亭(太)";
            }
            else if (combo.SelectedItem.ToString() == "西山")
            {
                SearchName = "洞庭西山";
            }
            else if (combo.SelectedItem.ToString() == "无锡南门")
            {
                SearchName = "无锡";
            }
            else if (combo.SelectedItem.ToString() == "琳桥")
            {
                SearchName = "漕桥";
            }
            else if (combo.SelectedItem.ToString() == "常熟枢纽")
            {
                SearchName = "常熟";
            }
            else if (combo.SelectedItem.ToString() == "望亭枢纽")
            {
                SearchName = "望亭(太)";
            }
            else if (combo.SelectedItem.ToString() == "太浦闸")
            {
                SearchName = "太浦匣(上)_";
            }
            else if(combo.SelectedItem ==null)
            {
                SearchName = Name;
            }
            else
            {
                SearchName = combo.SelectedItem.ToString();
            }
            int index = getlayer_id("重要站点");
            ILayer pLayer = axMapControl1.get_Layer(index);
            ISpatialFilter pSF = new SpatialFilterClass();   
            pSF.WhereClause = "STNM='" + SearchName + "'";
            IFeatureLayer pFL = pLayer as IFeatureLayer;
            IFeatureClass pFC = pFL.FeatureClass;
            axMapControl1.Map.ClearSelection();         
            IFeatureCursor pFCur = pFC.Search(pSF, true);
            IFeature pFeature = pFCur.NextFeature();
            //string name = pFeature.get_Value(nameindex).ToString();
            while (pFeature != null)
            {
                axMapControl1.Map.SelectFeature(pFL, pFeature);
                //目标窗口居中且放大
                IGeometry pGeo = pFeature.Shape;
                IEnvelope pEnv = pGeo.Envelope;
                if (pEnv.Height < 0.1)
                    pEnv.Expand(0.3, 0.3, false);
                else
                    pEnv.Expand(1.2, 1.2, true);
                (axMapControl1.Map as IActiveView).Extent = pEnv;
                //局部刷新
                axMapControl1.Refresh();
                IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
                viewRefresh.ProgressiveDrawing = true;
                viewRefresh.RefreshItem(pFL);
                pFeature = pFCur.NextFeature();
            }
        }
        public void Search()
        {
            string connString = "server=47.102.138.168;database=GIS;uid=GISJQQ;pwd=Jqq963741ArcObjects";
            SqlConnection SC = new SqlConnection(connString);
            SC.Open();
            SqlCommand SCmd = new SqlCommand();
            SCmd.Connection = SC;
            SCmd.CommandType = CommandType.Text;
            switch(a)
            {
                case 1:
                    SCmd.CommandText = "select * from TH5 where date>='" + pDTP.Value.ToString("yyyy-MM-dd") +
                "'and date<='" + pDTP2.Value.ToString("yyyy-MM-dd") + "'and Station='" + combo.SelectedItem + "'";
                    title = "太湖水位过程线" + '\n' + '\n' + pDTP.Value.ToShortDateString() + "到" + pDTP2.Value.ToShortDateString();
                    chart1.ChartAreas[0].AxisY.Title = "水位(单位：米)";
                    break;
                case 2:
                    SCmd.CommandText = "select * from YPSL_3 where date>='" + pDTP.Value.ToString("yyyy-MM-dd") +
                "'and date<='" + pDTP2.Value.ToString("yyyy-MM-dd") + "'and Station='" + combo.SelectedItem + "'";
                    title = "引排水量过程线" + '\n' + '\n' + pDTP.Value.ToShortDateString() + "到" + pDTP2.Value.ToShortDateString();
                    chart1.ChartAreas[0].AxisY.Title = "引排水量(单位：百万方)";
                    break;
                case 3:
                    SCmd.CommandText = "select * from ZDSW where date>='" + pDTP.Value.ToString("yyyy-MM-dd") +
                "'and date<='" + pDTP2.Value.ToString("yyyy-MM-dd") + "'and site='" + combo.SelectedItem + "'";
                    title = "重要站水位过程线" + '\n' + '\n' + pDTP.Value.ToShortDateString() + "到" + pDTP2.Value.ToShortDateString();
                    chart1.ChartAreas[0].AxisY.Title = "水位(单位：米)";
                    break;
                case 4:
                    SCmd.CommandText = "select * from ZDSW where date>='" + pDTP.Value.ToString("yyyy-MM-dd") +
               "'and date<='" + pDTP2.Value.ToString("yyyy-MM-dd") + "'and site='" + combo.SelectedItem + "'";
                    title = "重要站雨量过程线" + '\n' + '\n' + pDTP.Value.ToShortDateString() + "到" + pDTP2.Value.ToShortDateString();
                    chart1.ChartAreas[0].AxisY.Title = "降雨量(单位：毫米)";
                    break;
            }   
            SqlDataAdapter SDA = new SqlDataAdapter(SCmd);
            DataSet ds = new DataSet();
            SDA.Fill(ds, "table1");
            DataTable dt = ds.Tables["table1"];
            SDA.Dispose();
            SC.Close();
            panel1.Visible = true;
            chart1.Series.Clear();//清空chart
            chart1.Titles.Clear();      
            chart1.Titles.Add(title);
            Series lab = new Series();//初始化chart
            lab.BackSecondaryColor = Color.White;//设置背景颜色为白色
            lab.ChartType = SeriesChartType.Spline;//设置类型为折线图
            lab.MarkerStyle = MarkerStyle.Circle;
            chart1.ChartAreas[0].AxisX.Title = "年月";
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            dataGridView1.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                switch(a)
                {
                    case 1:
                        lab.Points.AddXY(dr["Date"], dr["WaterH"]);
                        dg.Rows.Add(dr["Date"], dr["Station"], dr["WaterH"]);
                        break;
                    case 2:
                        lab.Points.AddXY(dr["Date"], dr["YPSL"]);
                        dg.Rows.Add(dr["Date"], dr["Station"], dr["YPSL"]);
                        break;
                    case 3:
                        lab.Points.AddXY(dr["date"], dr["waterlevel"]);
                        dg.Rows.Add(dr["date"], dr["site"], dr["waterlevel"]);
                        break;
                    case 4:
                        lab.Points.AddXY(dr["date"], dr["rainfall"]);
                        dg.Rows.Add(dr["date"], dr["site"], dr["rainfall"]);
                        break;
                    case 5:
                        lab.Points.AddXY(dr["Date"], dr["WaterH"]);
                        dg.Rows.Add(dr["Date"], dr["Station"], dr["WaterH"]);
                        break;
                }  
            }
            chart1.Series.Add(lab);
            panel1.Controls.Add(chart1);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new System.Drawing.Point(-e.X, -e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
        }
        private void CreateContainer()
        {
            pDTP = new DateTimePicker();
            pDTP.Width = 130;
            pDTP.Height = 25;
            pDTP.Top = 297;
            pDTP.Left = 200;
            pDTP2 = new DateTimePicker();
            pDTP2.Width = 130;
            pDTP2.Height = 25;
            pDTP2.Top = 297;
            pDTP2.Left = 345;
            pDTP2.Visible = true;
            Label pLabel1 = new Label();
            Label pLabel2 = new Label();
            combo = new ComboBox();
            switch (a)
            {
                case 1:
                    pLabel1.Text = "站点选择:";
                    combo.Items.Add("望亭太");
                    combo.Items.Add("太浦口");
                    combo.Items.Add("西山");
                    combo.Items.Add("夹浦");
                    combo.Items.Add("小梅口");
                    combo.Items.Add("太湖平均");
                    break;
                case 2:
                    pLabel1.Text = "枢纽选择:";
                    combo.Items.Add("常熟枢纽");
                    combo.Items.Add("望亭枢纽");
                    combo.Items.Add("太浦闸");
                    break;
                case 3:
                    pLabel1.Text = "站点选择:";
                    combo.Items.Add("常州");
                    combo.Items.Add("无锡南门");
                    combo.Items.Add("杭长桥");
                    combo.Items.Add("甘露");
                    combo.Items.Add("嘉兴");
                    combo.Items.Add("平望");
                    combo.Items.Add("琳桥");
                    combo.Items.Add("苏州");
                    break;
                case 4:
                    pLabel1.Text = "站点选择:";
                    combo.Items.Add("常州");
                    combo.Items.Add("无锡南门");
                    combo.Items.Add("杭长桥");
                    combo.Items.Add("甘露");
                    combo.Items.Add("嘉兴");
                    combo.Items.Add("平望");
                    combo.Items.Add("琳桥");
                    combo.Items.Add("苏州");
                    break;
            }     
            pLabel1.Height = 25;
            pLabel1.Width = 70;
            pLabel1.Top = 300;
            pLabel1.Left = 1;
            pLabel1.Visible = true;
            pLabel2.Top = 301;
            pLabel2.Text = "-";
            pLabel2.Height = 25;
            pLabel2.Width = 20;
            pLabel2.Left = 330;
            pLabel2.Visible = true;
            Label plabel3 = new Label();
            plabel3.Text = "日期选择:";
            plabel3.Top = 300;
            plabel3.Height = 25;
            plabel3.Width = 68;
            plabel3.Left = 130;
            plabel3.Visible = true;
            chart1.Visible = true;
            Button btn = new Button();
            btn.Text = "查询";
            btn.Width = 50;
            btn.Height = 25;
            btn.Top = 295;
            btn.Left = 480;
            combo.Visible = true;
            combo.Width = 60;
            combo.Height = 25;
            combo.Top = 298;
            combo.Left = 70;
            btn.Visible = true;
            pDTP.Visible = true;
            dg = new DataGridView();
            dg.Top = 320;
            dg.Left = 1;
            dg.Width = 540;
            dg.ColumnCount = 3;
            dg.ColumnHeadersVisible = true;
            dg.Columns[0].Name = "日期";
            dg.Columns[1].Name = "站点";
            dg.Columns[2].Name = "数值";
            dg.Columns[0].Width = 180;
            dg.Columns[1].Width = 180;
            dg.Columns[2].Width = 180;
            dg.BackgroundColor = Color.SlateBlue;
            dg.Columns[0].DefaultCellStyle.Format = "yyyy-MM-dd";
            panel1.Controls.Add(dg);
            panel1.Controls.Add(plabel3);
            panel1.Controls.Add(combo);
            panel1.Controls.Add(chart1);
            panel1.Controls.Add(pDTP);
            panel1.Controls.Add(pDTP2);
            panel1.Controls.Add(pLabel1);
            panel1.Controls.Add(pLabel2);
            panel1.Controls.Add(btn);
            panel1.Visible = true;
            btn.Click += new EventHandler(Btn_Click);//使用事件函数句柄指向一个具体的函数    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            a = 3;
            this.panel1.Controls.Clear();
            chart1.Titles.Clear();
            chart1.Series.Clear();
            CreateContainer();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            a = 4;
            this.panel1.Controls.Clear();
            chart1.Titles.Clear();
            chart1.Series.Clear();
            CreateContainer();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            a = 2;
            this.panel1.Controls.Clear();
            chart1.Titles.Clear();
            chart1.Series.Clear();
            CreateContainer();
        }



        private void chart1_Click(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            chart1.Titles.Clear();
            chart1.Series.Clear();
            this.panel1.Visible = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            b = 1;
            dataGridView2.Rows.Clear();
            UpdateTable();
            //高亮居中显示
            string Name = comboBox7.Text;
            if (Name == "太浦口")
            {
                SearchName = "大浦口";
            }
            else if (Name == "望亭太")
            {
                SearchName = "望亭(太)";
            }
            else if (Name == "西山")
            {
                SearchName = "西塘";
            }
            else if (Name == "无锡南门")
            {
                SearchName = "无锡";
            }
            else if (Name == "琳桥")
            {
                SearchName = "漕桥";
            }
            else if (Name == "常熟枢纽")
            {
                SearchName = "常熟";
            }
            else if (Name == "望亭枢纽")
            {
                SearchName = "望亭(太)";
            }
            else if (Name == "太浦闸")
            {
                SearchName = "太浦匣(上)_";
            }
            else
            {
                SearchName = Name;
            }           
            int index = getlayer_id("重要站点");
            ILayer pLayer = axMapControl1.get_Layer(index);
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.WhereClause = "STNM='" + SearchName + "'";
            IFeatureLayer pFL = pLayer as IFeatureLayer;
            IFeatureClass pFC = pFL.FeatureClass;
            axMapControl1.Map.ClearSelection();
            IFeatureCursor pFCur = pFC.Search(pSF, true);
            IFeature pFeature = pFCur.NextFeature();
            //string name = pFeature.get_Value(nameindex).ToString();
            while (pFeature != null)
            {
                axMapControl1.Map.SelectFeature(pFL, pFeature);
                //目标窗口居中且放大
                IGeometry pGeo = pFeature.Shape;
                IEnvelope pEnv = pGeo.Envelope;
                if (pEnv.Height < 0.1)
                    pEnv.Expand(0.3, 0.3, false);
                else
                    pEnv.Expand(1.2, 1.2, true);
                (axMapControl1.Map as IActiveView).Extent = pEnv;
                //局部刷新
                axMapControl1.Refresh();
                IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
                viewRefresh.ProgressiveDrawing = true;
                viewRefresh.RefreshItem(pFL);
                pFeature = pFCur.NextFeature();
            }
            axMapControl1.ActiveView.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            b = 3;
            dataGridView1.Rows.Clear();
            UpdateTable();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.SelectedIndex==0)
            {
                comboBox7.Items.Clear();
                comboBox7.Items.Add("望亭太");
                comboBox7.Items.Add("太浦口");
                comboBox7.Items.Add("西山");
                comboBox7.Items.Add("夹浦");
                comboBox7.Items.Add("小梅口");
                comboBox7.Items.Add("太湖平均");
            }
            else if(comboBox6.SelectedIndex==1||comboBox6.SelectedIndex==2)
            {
                comboBox7.Items.Clear();
                comboBox7.Items.Add("常州");
                comboBox7.Items.Add("无锡南门");
                comboBox7.Items.Add("杭长桥");
                comboBox7.Items.Add("甘露");
                comboBox7.Items.Add("嘉兴");
                comboBox7.Items.Add("平望");
                comboBox7.Items.Add("琳桥");
                comboBox7.Items.Add("苏州");
            }
            else
            {
                comboBox7.Items.Clear();
                comboBox7.Items.Add("常熟枢纽");
                comboBox7.Items.Add("望亭枢纽");
                comboBox7.Items.Add("太浦闸");
            }
        }


        private void button0_Click(object sender, EventArgs e)
        {
            //绘制过程线变换底图
            axMapControl1.Extent = axMapControl1.FullExtent;
            draw = 1;
            h = 1;
            int index = 0;
            index = getlayer_id("重要站点");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = true;

            index = getlayer_id("水文站");
            ILayer pLayer5 = axMapControl1.get_Layer(index);
            pLayer5.Visible = false;

            index = getlayer_id("雨量站");
            ILayer pLayer6 = axMapControl1.get_Layer(index);
            pLayer6.Visible = false;

            index = getlayer_id("太湖流域高程图");
            ILayer pLayer1 = axMapControl1.get_Layer(index);
            pLayer1.Visible = false;

            index = getlayer_id("铁路");
            ILayer pLayer3 = axMapControl1.get_Layer(index);
            pLayer3.Visible = false;

            index = getlayer_id("水质监测站点");
            ILayer pLayer4 = axMapControl1.get_Layer(index);
            pLayer4.Visible = false;
            axMapControl1.ActiveView.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            draw = 0;
            h = 0;
            axMapControl1.Extent = axMapControl1.FullExtent;
            //取消绘制过程线
            flag = 0;
            int index = 0;
            index = getlayer_id("太湖流域高程图");
            ILayer pLayer1 = axMapControl1.get_Layer(index);
            pLayer1.Visible = true;

            index = getlayer_id("铁路");
            ILayer pLayer3 = axMapControl1.get_Layer(index);
            pLayer3.Visible = true;

            index = getlayer_id("水质监测站点");
            ILayer pLayer4 = axMapControl1.get_Layer(index);
            pLayer4.Visible = true;
            axMapControl1.ActiveView.Refresh();

            index = getlayer_id("重要站点");
            ILayer pLayer = axMapControl1.get_Layer(index);
            pLayer.Visible = false;

            index = getlayer_id("水文站");
            ILayer pLayer5 = axMapControl1.get_Layer(index);
            pLayer5.Visible = true;

            index = getlayer_id("雨量站");
            ILayer pLayer6 = axMapControl1.get_Layer(index);
            pLayer6.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.StartPosition = FormStartPosition.CenterScreen;
            fm2.Show();
        }

       

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
