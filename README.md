# JoziIncident
A small municipal service application designed to help customers:
- Report and view incidents
- View upcoming municipal events
- View status of service requests
## Changes Made
- Service request button now enabled. Users can now view their service requests by clicking on the "Service Request" button on the left side of the main window
- A progress bar has been added to help users visualise the progress of their serive request.
- Related service requests will be show and indicated when searching for a specific service request.
- The file size of the project has been reduced from around 400MB to around a 150MB uncompressed by removing unnessary libraries.
- User date search dates are now considered in the local events & announcements section, but only dates that have been searched more than 3 times. Reason being, to avoid recommendation outliers from being included in the recommendations regarding the date. For example, a user might search for specific dates ones or twice and then never again, such a pattern might screw up the recommendations.
## Data Structures Used
### Binary Search Tree (BST)
A BST is a data structure used to store elements in a hierarchical structure that consists of at least one node, one left subtree and one right subtree. 
#### Insertion
The BST in the context of this project was used to efficiently insert new service request objects while maintaining a sorted order based on the request id of each service request. For example, if a customer requests for a service over the phone, the municipality employee can hard-code the details of that service request into an object and pass that object into the BST class's "Insert" method and that service request will now appear in the service request page.
#### Searching
The BST was used to efficiently search for specific service requests using a provided service request id variable. For example, using the search bar on the service request page, the user can search for a service request with the id "REQ00001" and the system will display the details of that service request in a visually appealing manner.
#### Updates
The BST also contains a method to update the status of a service request using the id of the service request that needs to be modified. For example, if a technician is on his way to repair a electricity meter connection at a customer's house, we can change the status of that service request from "Pending" to "In Progress" using the service requests id and pass that id to the "UpdateStatus" method in our BST class. Although this was not used in the end, the method is still there and can be used in future iterations.
#### Traversal
The BST was traversed using the in-order-traversal method which allowed us to retrieve a sorted list of all service requests.
### Graph
A Graph is a flexible data structure that represents objects and their connections. The graph was used to represent dependencies between service requests, where one request might depend on the completion of another. Something that cannot be easily represented using the likes of a binary search tree or an array. 
#### Establishing Dependencies
The graph class contains a "AddDependency" method that uses a dictionary to store each service request's dependent requests, this allows us to efficiently track which request depend on other requests. For example, before your electricity meter can be activated (job 1), technicians need to do the wiring for that electricity meter first (job 2) so in our application, we will add a dependancy for job 1 onto job 2 using the "AddDependency" method which takes two parameters, one for the request that is dependent and one for the request that is independent.
#### Dependency Lookup
Depth-first-search (DFS) is a well known graph algorithm that explores all nodes and edges of a graph by going as deep as possible along each branch before backtracking. This means that the algorithm starts at a given node and goes as far down a branch as possible before returning to explore other branches. In the method "GetDependencies", the graph uses DFS to find and retrieve all dependent requests for a specific service request. The DFS method retrieves directly and indirectly related requests of a given request.
## Minimum System Requirements
- A dual core processor @ 1.60GHz
- Integrated graphics
- 4GB RAM
- Windows 10 version 22H2 19045.5011 or higher
- 200MB Free Disk space or more
## Software Requirements
- Visual Studio Community 2022 17.11.5 or higher
## Instructions
- Click on the green "code" drop down menu
- Select "Download ZIP"
- Extract downloaded file titled "prog7312-part1-ST10091324-main" using an unzipping tool like 7zip or winrar
- Once unzipped, double left click on the "ST10091324_PROG7312_Part1" folder
- Double left click on "ST10091324_PROG7312_Part1.sln" visual studio solution file
- Wait for Visual Studio and all necessary components to be loaded
- Hold Ctrl + F5 key to start the solution file without debugging
- The program might ask you to trust one or more certificates, click on the yes option
- The web application will then open in your default browser
- If you are on a windows machine, locate your default browser on your taskbar and click on it. It should be highlighted in an orange colour
- If you are on an ios machine, locate your default browser on your dock and click on it. It should be jumping up and down on your dock
- If you are on a linux machine, locate your default browser on your dock and click on it. It should be highlighted.
- You will then be welcomed by a login screen
- To safely close the program use the close button located on the top right side of your default browser
## Proof of Work
- To view PoW video locate the folder titled "Recording Proof"
- Double left click on that folder
- Double left click on the .mvk file inside that folder and the video should start playing
## SQL Script
To view the SQL script, locate the folder titled "SQL_Scripts" and:
- Double left click on that folder
- Left click on the sql file titled, "Database_creations.sql"
- Drag and drog it into visual studio (once VS is open)
- Click on the green run button located on the left top side or press following shortcut simultaneously (Ctrl + Shift + E)
- Wait for it to complete executing and run the project again.
### Side Notes
- Due to compression and recording software, the colours palatte on the video does not perfectly reflect the colours of the original source
 But its good enough to get an idea
- If your machine does not support .mkv files, download VLC from here: https://www.videolan.org/vlc/
- Alternatively, use a .mkv to .mp4 converter such as FreeConvert, available here: https://www.freeconvert.com/mkv-to-mp4
