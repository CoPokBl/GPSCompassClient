/*

This file injects basic navigation into the page.
Every normal page that should contain navigation and footer
elements should include this file using a script tag.

The header and footer tags should look like:
<div id="site-nav"></div>
        and
<div id="site-footer"></div>

This file requires that all HTML files importing it has a meta tag:
<meta name="ptr" id="pathToRoot" content="../">
That contains the path to the root of the website.

Depends on:
- global.js

 */

// Get where the root directory is in relation to the HTML file
let pathToRoot;
const ptrObj = document.getElementById("pathToRoot");
if (ptrObj === null) {
    console.warn("No meta tag with id 'pathToRoot' found. It is highly recommended to have this tag. (Defaulting to '../')");
    pathToRoot = "../";
} else {
    pathToRoot = ptrObj.getAttribute("content");
    console.log("Root: " + pathToRoot);
}

// Inject navbar into site at div with id 'site-nav'
const nav = document.getElementById('site-nav');

if (nav == null) {
    // Navbar isn't found
    console.log('Navbar element doesn\'t exist');
} else {
    // Put the navbar code into this element
    let navBarCode = `

<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
       <a class="navbar-brand" href="%%root%%"><img src="%%root%%assets/images/icon.png" width="41" height="41"></a>
       <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
       <span class="navbar-toggler-icon"></span>
       </button>
       <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav mr-auto">
          
             <li class="nav-item active">
                <a class="nav-link" href="%%root%%">Home</a>
             </li>
             
             <li class="nav-item">
                <a class="nav-link" href="%%root%%status">Status</a>
             </li>
             
             <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Info
                </a>
                <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                   <a class="dropdown-item" href="%%root%%events/">Upcoming Events</a>
                   <a class="dropdown-item" href="%%root%%discord/">Discord</a>
                   <a class="dropdown-item" href="%%root%%forums/">Forums</a>
                   <a class="dropdown-item" href="%%root%%combat/">Combat System</a>
                </div>
             </li>

          </ul>

       </div>
    </nav>

`;

    // Replace %%root%% with the path to the root of the website
    navBarCode = replaceAll(navBarCode, "%%root%%", pathToRoot);

    // Inject the navbar code into the navbar element
    nav.innerHTML = navBarCode;

    console.log("Navbar injected");
}

// Inject footer into site at div with id 'site-footer'
const footer = document.getElementById('site-footer');

if (footer == null) {
    // Footer isn't found
    console.log('Footer element doesn\'t exist');
} else {
    // Put the footer code into this element
    let footerCode = `

<div class="row">
    <div class="text-center col-lg-6 offset-lg-3">
        <h4>Serble Site </h4>
        <p>Copyright CoPokBl &copy; 2020-2022 &middot; All Rights Reserved &middot; <a href="%%root%%">Serble&nbsp;</a></p>
    </div>
</div>

    `;

    // Replace %%root%% with the path to the root of the website
    footerCode = replaceAll(footerCode, "%%root%%", pathToRoot);

    // Inject the navbar code into the navbar element
    footer.innerHTML = footerCode;

    console.log("Footer injected");
}

console.log("Navbar and footer injection complete");