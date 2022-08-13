# Power BI 3D
Custom Visual for **Power BI** to visualize 3D models and connect them to your data. It uses the library three.js and its fantastic [3DM loader](https://threejs.org/docs/#examples/en/loaders/3DMLoader) to visualize 3D models in power BI. It currently only supports Rhino3d models, but the visual can be easily expanded to support other file formats.

[![Watch the video][https://api.onedrive.com/v1.0/shares/u!aHR0cHM6Ly8xZHJ2Lm1zL3UvcyFBcnBiTGlJYngxNVFrOFZXOFpHdzh0RVkzVEpINkE_ZT1JZ1JuMlY/root/content](https://youtu.be/CE_XmEtYdaU)

<br />

## Acknowledgement
This custom visual was developed by Diego Apellániz.<br/> <br/> 
Thanks to [McNeel](https://discourse.mcneel.com/t/3dmloader-for-three-js/107702) for the wonderful 3DM loader for three.js.<br/> <br/> 
Thanks to Tizian Alkewitz for the intensive beta testing.

## How to use
### 1) Add custom visual to Power BI
1) Download last [release](https://github.com/diego-apellaniz/PowerBI3D/releases) of this repository.
2) Download and install [Power BI Desktop](https://www.microsoft.com/store/productId/9NTXR16HNW1T).
3) Open Power BI, create a new file and go to *Files -> Import -> Power BI Visual from File* and select *3DVisor.pbiviz* from the ***downloaded files***.
4) Create a 3D Visor in Power BI by select the imported visual from the visualizations panel on the right side. It won't display anything though until we connect it to our data and upload the 3D model. <br />

### 2) Add data to your Power BI dashboard
1) Prepare a Excel table with the following format. One column must contain the GUIDs of the objects of your 3D model you want to import in Power BI. Additional columns may include different categories for the GUIDs and different values. It's important that all cells of the table are filed:

![Image](https://github.com/diego-apellaniz/PowerBI3D/blob/main/Images/excel_lca.png)

   If you are using Rhino3D as the source of your 3D model, you can easily use Grasshopper to create your Excel table:
   
![Image](https://github.com/diego-apellaniz/PowerBI3D/blob/main/Images/gh_ids.png)

2) Upload Excel table to Power BI
3) Connect GUIDs with the 3D Visor. The input form to introduce the URL of your 3D model should appear now in the 3D Visor.

### 3) Import 3D model in Power BI
1) Post-process your 3D modell. Delete the elements you don't want to import in Power BI and you didn't include in the GUIDs column of your table. In case you're using a Rhino model, it's strongly recommended to convert all geometry elements to **meshes**, since other geometry types may not be implemented in the 3DM loader yet.
2) Get your 3D model online. Only 3D models uploaded to a https domain with CORS enabled can be imported in Power BI. In case you don't have access to such a domain, you can use one of the following resources:</br>

#### Localhost
Make your 3D model locally accessible through yor **Localhost** IP adress. In the downloaded files from the last [release](https://github.com/diego-apellaniz/PowerBI3D/releases), you will find a Python script to make files accessible through a localhost IP adress. In order to use it, we need to create an SSL certificate first.</br>
1) Install [OpenSSL](https://slproweb.com/products/Win32OpenSSL.html).</br>
2) Create an SSL certificate file. We need it to create an https server on Localhost as shown in this [video](https://youtu.be/f9ZadlfSIDI). In the corresponding steps, you can use the provided *config.txt* file and copy the command line in the file *command.txt* from the downloaded folder. On [4:16](https://youtu.be/f9ZadlfSIDI?t=256) stop the video. You should instead look on to how to [load your generated certificate *localhost.crt* into Google Chrome](https://docs.vmware.com/en/VMware-Adapter-for-SAP-Landscape-Management/2.1.0/Installation-and-Administration-Guide-for-VLA-Administrators/GUID-D60F08AD-6E54-4959-A272-458D08B8B038.html) or another browser. Finally open the provided *certfile.pem* file from the downloaded folde with the text editor. Replace the contents of the KEY and CERTIFICATE with the content of the previously generated *localhost.key* and *localhost.crt*.
3) Install [Python](https://www.python.org/downloads/) in your computer.</br>
4) Open the Windows Command Promt as an administrator, type *cd PATH* where PATH is the route to the folder of the downloaded python script and tpye *pip install -r requirements.txt*.
5) Now you can finally run the python script *ActivateLocalHost.py* by doing double click into it. You should get the url of the https server runing in your localhost. Something like `https://localhost:49379`.</br>
6) Enter this url in Google Chrome. You shouldn't receive any warning if the SSL certificate and the file *certfile.pem* were created properly. Now you should see in the browser all files located in the same folder as the python script. You can also copy here the 3D model you want to import. You can then refresh the browser and copy the link to the file of the 3d model which you will then enter in Power BI!</br>

#### OneDrive
We can also use a link to a file stored in OneDrive. SharePoint and OneDrive for business unfortunately don't work here.</br>
1) Get the [shared link](https://www.dummies.com/article/technology/computers/operating-systems/windows/windows-10/how-to-share-a-onedrive-link-140260/) to the file in OneDrive.</br>
2) Run the program *EncodeOneDriveURL.exe* from the downloaded files and enter the shared link from OneDrive. You will get a resulting url that is the direct link to the file and that you can use in Power BI!

### 4) Connect 3D model to your data
Just connect your imported data to the 3D visor. If just the GUIDS are provided, the 3D model will be displayed in gray. If a category is also provided, the different groups wil be displayed according to the color pallette of Power BI. If also data for the Colors parameter is provided, the model objects will be displayed using a color gradient that interpolates the added values of each group. Finally, if Color dara is provided with no grouping, the color gradient will consider the value of each independent model object with no grouping.

![Image](https://github.com/diego-apellaniz/PowerBI3D/blob/main/Images/mapping_data.png)
