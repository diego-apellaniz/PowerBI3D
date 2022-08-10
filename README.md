# Power BI 3D
Custom Visual for **Power BI** to visualize 3D models and connect them to your data. It uses the library three.js and its fantastic [3DM loader](https://threejs.org/docs/#examples/en/loaders/3DMLoader) to visualize 3D models in power BI. It currently only supports Rhino3d models, but the visual can be easily expanded to support other file formats.
<br />

## Acknowledgement
This custom visual was developed by Diego Apellániz.<br/> <br/> 
Thanks to [McNeel](https://discourse.mcneel.com/t/3dmloader-for-three-js/107702) for the wonderful 3DM loader for three.js.

## How to use</br>
### Add custom visual to Power BI
1) Download last [release](https://github.com/diego-apellaniz/PowerBI3D/releases) of this repository.
2) Download and install [Power BI Desktop](https://www.microsoft.com/store/productId/9NTXR16HNW1T).
3) Open Power BI, create a new file and go to *Files -> Import -> Power BI Visual from File* and select *3DVisor.pbiviz* from the ***downloaded files***.
4) Create a 3D Visor in Power BI by select the imported visual from the visualizations panel on the right side. It won't display anything though until we connect it to our data and upload the 3D model.
<br />
### Add data to your Power BI dashboard
1) Prepare a Excel table with the following format. One column must contain the GUIDs of the objects of your 3D model. Additional columns may include different categories for the GUIDs and different values. It's important that all cells of the table are filed:

If you are using Rhino3D as the source of your 3D model, you can easily use Grasshopper to create your Excel table:

2) Upload Excel table to power BI
3) Connect GUIDs with the 3D Visor. The input form to introduce the URL of your 3D model should appear now in the 3D Visor.

### Import 3D models in Power BI
The current version
<br />

## Connect 3D model to your data


## Enable support for other file formats

