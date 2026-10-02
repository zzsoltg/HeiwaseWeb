# HeiwaseWeb — Complete GitHub Backlog

> Snapshot exported 2026-10-02 from the repository’s GitHub issues. Includes open and closed issues and native sub-issue relationships. Issue bodies are reproduced verbatim.

## Export notes

- Issues are grouped by milestone and listed in ascending issue-number order; native sub-issues are nested under their parent issue.
- The available issue records expose milestone titles but not milestone description text. No milestone descriptions are invented here; see each issue’s full body below.
- Issue states reflect the GitHub issue records at export time.

## Milestones and issues

1. **M1: Bugfixes for first deployment**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 8 (including sub-issues; 0 open, 8 closed)

   1. **[#1 — Picture responsivity](https://github.com/zzsoltg/HeiwaseWeb/issues/1)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         The "Eseménynaptár" picture's size is based on the display. It reduces on mobile and smaller screens, enhances on desktop and TV. The picture keeps its content's readability.
         ```

   2. **[#2 — Refactor code structure](https://github.com/zzsoltg/HeiwaseWeb/issues/2)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         Requirements:
         - Every class, enum or other type should have its own file.
         - Code structure should follow SOLID principles.
         - No inline CSS allowed. Only CSS class declaration, then separate CSS file for the content.
         - JavaScript files and code snippets should be replaced to wwwroot/js folder, and inject its funcionality via Blazor's solutions.
         ```

   3. **[#3 — Fix form layout](https://github.com/zzsoltg/HeiwaseWeb/issues/3)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         The form layout should appear as expected using the proper font designs.
         ```

   4. **[#4 — Fix hall of fame section](https://github.com/zzsoltg/HeiwaseWeb/issues/4)** — `CLOSED`
      - **Labels:** bug, enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         The hall of fame section supposed to switch components via animations, and the pictures, descriptions should be changed, new people should be added.
         ```

   5. **[#5 — Integrate Facebook and Instagram APIs.](https://github.com/zzsoltg/HeiwaseWeb/issues/5)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         The social media section should work and load the club's feed.
         ```

   6. **[#6 — Change the text](https://github.com/zzsoltg/HeiwaseWeb/issues/6)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         The text's applied on the page should be changed, followed marketing instructions. Requires further coservation.
         ```

   7. **[#10 — Favicon](https://github.com/zzsoltg/HeiwaseWeb/issues/10)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         Set a favicon for the page.
         ```

   8. **[#11 — Give form backend](https://github.com/zzsoltg/HeiwaseWeb/issues/11)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         The form should send an email to info@szegedkarate.hu with the given details, and should raise a toastr message in case of a successful and unsuccessful send.
         ```

2. **M2: Content, UX & Design System Refresh**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 17 (including sub-issues; 0 open, 17 closed)

   1. **[#7 — Localization](https://github.com/zzsoltg/HeiwaseWeb/issues/7)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         Apply a localization technique for Blazor WASM Standalone pages. Store the user's culture in the cookies.
         ```

   2. **[#8 — Modals](https://github.com/zzsoltg/HeiwaseWeb/issues/8)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         Some of the information about the club should appear in modals, when a user clicks on the components. The following should implemented this way:
         - Training types.
         - CVs about the coaches.
         - CVs of the senpais and competitors.
         ```

   3. **[#9 — Rebuild the Gallery with a slider and modal viewer](https://github.com/zzsoltg/HeiwaseWeb/issues/9)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a site visitor, I want a picture slider that opens full-size modals so that I can browse club photos and read training/event descriptions without leaving the page.
         
         ### Description
         Upgrade the existing static gallery section on the `Heiwase.App.Blazor` public landing page. Implement a carousel/slider component for the image thumbnails. When a user clicks a thumbnail, it must open a modal (dialog) displaying the high-resolution image alongside its long-form training/event description. This improves the UX by cleaning up the main page flow and hiding long blocks of text until requested.
         
         ### Acceptance Criteria (AC)
         Given the user is viewing the gallery section,
         When they click on a thumbnail image in the slider,
         Then a modal opens displaying the full-size image and its associated description text.
         
         Given the opened gallery modal,
         When the user clicks the "X" button or clicks outside the modal area,
         Then the modal closes and the user remains in their current scroll position on the landing page.
         
         ### Definition of Done (DoD)
         - [x] Image slider component is integrated into the gallery area.
         - [x] Click-to-open modal logic is implemented in Blazor.
         - [x] Long-form training and event descriptions are moved out of the inline page text and into the modal content.
         - [x] The slider and modal are fully responsive and function correctly on mobile devices.
         ```

   4. **[#14 — Fix animations](https://github.com/zzsoltg/HeiwaseWeb/issues/14)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         Animation delay doesn't work. Either solve it by following coding conventions or refactor by removing delay feature completely.
         Add mobile navbar appearing and disappearing animations.
         Fix form animations
         ```

   5. **[#21 — Remove focus at hero title](https://github.com/zzsoltg/HeiwaseWeb/issues/21)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         When starting the application a focus border appears on the hero title. Remove it.
         ```

   6. **[#24 — Convert picture files](https://github.com/zzsoltg/HeiwaseWeb/issues/24)** — `CLOSED`
      - **Labels:** bug, enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         The pictures should be converted to webp or other web image type. This is to enhance website loading performance on low connection devices.
         ```

   7. **[#27 — Tripled text](https://github.com/zzsoltg/HeiwaseWeb/issues/27)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         In the Our History Dialog's English version one picture description appears three times. AC: only one description remains.
         ```

   8. **[#28 — Replace placeholder media with production-quality assets](https://github.com/zzsoltg/HeiwaseWeb/issues/28)** — `CLOSED`
      - **Labels:** enhancement, wontfix
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Product Owner, I want all placeholder media replaced with high-quality, approved assets so that the website presents a professional marketing image.
         
         ### Description
         Source and apply the previously agreed-upon high-quality pictures for all content sections where a visual upgrade is necessary. The media pipeline must be standardized: normalize all selected images (and any remaining placeholders) to a baseline JPG format, then generate optimized WebP and AVIF variants. Update all `<picture>` tags in the Blazor components to point to these new paths. Finally, re-encode and replace the hero background video with a higher-quality version of the same content.
         
         ### Acceptance Criteria (AC)
         Given a visitor is viewing any section with imagery,
         When the images load,
         Then they fit perfectly within their designated UI frames without distortion and appear in high resolution.
         
         Given a modern web browser,
         When it parses the HTML,
         Then it correctly loads the optimized AVIF or WebP format via the `<picture>` tag, falling back to JPG only if unsupported.
         
         Given the updated media files are deployed,
         When inspecting the site layout across different screen sizes,
         Then there are no visual regressions, and the original design system remains intact.
         
         ### Definition of Done (DoD)
         - [ ] Approved high-quality images are selected for each content section.
         - [ ] All images are normalized to JPG, and converted into both WebP and AVIF formats.
         - [ ] Blazor components are updated with the new media paths in `<picture>` tags.
         - [ ] The hero background video is replaced with a higher-quality encode.
         - [ ] UI framing and overall layout are manually tested and verified for zero regressions.
         ```

   9. **[#29 — Rewrite marketing copy across all sections](https://github.com/zzsoltg/HeiwaseWeb/issues/29)** — `CLOSED`
      - **Labels:** enhancement, wontfix
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Product Owner, I want the text content across all sections of the public site to be rewritten with a professional and catchy tone so that the club is presented attractively to prospective members and partners.
         
         ### Description
         Review and rewrite the existing placeholder or draft text across the entire `Heiwase.App.Blazor` public landing page (Hero, News, Dojo, Gallery, Contact, etc.). The new copy must align with the Product Owner's branding guidelines, ensuring a consistent, engaging, and professional voice that effectively communicates the values of the karate club.
         
         ### Acceptance Criteria (AC)
         Given a prospective member visiting the landing page,
         When they read the hero section and value propositions,
         Then the messaging is clear, catchy, professional, and free of placeholder text.
         
         Given the updated copy is implemented in the Blazor components,
         When the Product Owner reviews the deployed preview environment,
         Then they can sign off that the tone meets the marketing requirements.
         
         ### Definition of Done (DoD)
         - [ ] Copy is rewritten for all static UI components (Hero, About, Dojo, Contact).
         - [ ] Text is updated directly in the respective `.razor` files.
         - [ ] The final text is reviewed and approved by the Product Owner.
         - [ ] A spell-check and grammar review has been completed with zero remaining errors.
         ```

   10. **[#33 — Build out the Dojo section](https://github.com/zzsoltg/HeiwaseWeb/issues/33)** — `CLOSED`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a site visitor, I want a dedicated section describing the dojo(s) so that I know exactly where, when, and how the training happens before I decide to join.
         
         ### Description
         Implement a new UI component (e.g., `DojoSection.razor`) on the `Heiwase.App.Blazor` public landing page. This section must provide clear, structured information about the club's training locations, schedules, and the general training environment. It should seamlessly integrate with the existing LESS design system and provide a responsive layout for mobile users.
         
         ### Acceptance Criteria (AC)
         Given a prospective student is browsing the landing page,
         When they scroll to or click the "Dojo" link in the navigation menu,
         Then they are presented with a dedicated section displaying the training address, map link/placeholder, and schedule.
         
         Given the user is viewing the site on a mobile device,
         When the Dojo section is rendered,
         Then the content (text, schedule tables, images) stacks or adjusts responsively without breaking the viewport width.
         
         ### Definition of Done (DoD)
         - [x] `DojoSection.razor` component is created and added to the main page layout.
         - [x] Accurate location and scheduling details are implemented.
         - [x] The component is styled using existing LESS variables and mixins for visual consistency.
         - [x] Responsive design is tested and passes on both desktop and mobile viewports.
         ```

   11. **[#34 — Fix and extend section-entry animations](https://github.com/zzsoltg/HeiwaseWeb/issues/34)** — `CLOSED`
      - **Labels:** bug, enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a site visitor, I want smooth and bug-free entrance animations as I navigate the site so that the overall browsing experience feels polished, modern, and premium.
         
         ### Description
         Address and resolve the existing CSS animation-delay bug on section entries (either by fixing the timing logic or removing the delay entirely to align with modern web design conventions). Additionally, implement a smooth show/hide CSS transition for the mobile navigation bar (hamburger menu), and polish the entrance and interaction animations on the contact application form to ensure visual consistency.
         
         ### Acceptance Criteria (AC)
         Given the user is scrolling down the public landing page,
         When a new section (e.g., Dojo, Gallery) enters the viewport,
         Then the elements animate into place smoothly without the previous jarring delay bug.
         
         Given a user is browsing on a mobile device,
         When they tap the hamburger menu icon to open or close the navigation,
         Then the menu expands and collapses with a smooth, visually pleasing transition.
         
         Given the contact application form,
         When the form is rendered or interacted with (e.g., focus states, validation errors),
         Then the animations play correctly and do not break the page layout.
         
         ### Definition of Done (DoD)
         - [x] The section-entry animation-delay bug is either fixed or removed from the LESS/CSS files.
         - [x] Mobile navbar show/hide transition is implemented.
         - [x] Contact form animations are refined and polished.
         - [x] Animations are tested and perform smoothly on both desktop and mobile browsers (Chrome, Safari, Edge) without causing horizontal scrolling or layout shifts.
         ```

   12. **[#35 — No border radius for training schedule](https://github.com/zzsoltg/HeiwaseWeb/issues/35)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         ### Story
         Me as a Product Owner, I want the training schedule to have a border radius, to have a unified look like all other components.
         
         ### Details
         - Set a border-radius property to the value it is used to have.
         
         ### AC
         - [ ] Border radius reappears on the training schedule picture
         - [ ] No regression caused in other parts of the project
         ```

   13. **[#107 — Reduce Static Media Size to Unblock Azure SWA Deployment](https://github.com/zzsoltg/HeiwaseWeb/issues/107)** — `CLOSED`
      - **Labels:** bug, wontfix
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a developer, I want to temporarily reduce the size of the static media assets so that the application size falls below the 250 MB Azure Free Tier limit and the GitHub Actions deployment can succeed.
         
         ### Description
         The GitHub Actions deployment fails at the final step because the compiled `Heiwase.App.Blazor/wwwroot` folder exceeds the 250 MB (262,144,000 bytes) strict size limit of the Azure Static Web Apps Free plan (currently sitting at ~256 MB). 
         Before the full Azure Blob Storage architecture (Epic 2.2) is implemented for media management, a temporary hotfix is required. The solution is to delete one of the redundant hero background video formats (e.g., the `.webm` fallback) and trim the remaining video (e.g., `.mp4`) to play only a shorter segment of the montage.
         
         ### Acceptance Criteria (AC)
         Given the local `wwwroot` directory,
         When inspecting its total size after the media changes,
         Then the total directory size is strictly under 250 MB (preferably < 240 MB for safety margin).
         
         Given the GitHub Actions CI/CD pipeline,
         When the trimmed media commit is pushed to the `master` branch,
         Then the Azure Static Web Apps deployment step completes successfully without the "size of the app content was too large" error.
         
         ### Definition of Done (DoD)
         - [x] One redundant hero video format is deleted from the repository.
         - [x] The remaining hero video is trimmed/cut to a significantly smaller file size.
         - [x] The HTML `<video>` or `<picture>` tags are updated to reflect the removed/changed files.
         - [x] `wwwroot` total size is verified locally.
         - [ ] Code is pushed and the automated deployment succeeds.
         ```

      1. **[#109 — Remove avif and webp files until final blob deployment](https://github.com/zzsoltg/HeiwaseWeb/issues/109)** — `CLOSED`
         - **Labels:** bug, wontfix
         - **Full description (verbatim issue body):**
            ```markdown
            ### User story
            As a developer, I want to remove avif and webp files from source code, to make the Azure Static Web App running.
            
            ### Description
            Remove all of the avif and webp files from `wwwroot` folder. Modifiy the source code and comment out code sections in relation with loading avif and webp files.
            
            ### Acceptance Criteria (AC)
            Given the local `wwwroot` directory,
            When looking into `img `folder,
            Then no avif or webp files are listed.
            
            Given any razor file in source code,
            When there is any `<picture>` tag inside,
            Then only `<img>` tags can be found inside.
            
            ### Definition of Done (DoD)
            
            - [ ] All files removed.
            - [ ] No picture elements with avif or webp can be found in source code.
            - [ ] Azure CI/CD pipline build succeeds.
            ```

      2. **[#112 — Update Missing LESS File Asset Paths to Blob Storage URLs](https://github.com/zzsoltg/HeiwaseWeb/issues/112)** — `CLOSED`
         - **Labels:** bug
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a frontend developer, I want to update the background image paths in the LESS files to point to the new Azure Blob Storage and set the encoding correctly in Azure, so that the UI does not break and background images load correctly after removing the local assets and the text in the website is readable.
            
            ### Description
            During the manual offloading of large media assets to Azure Blob Storage (to bypass the 250 MB SWA limit), the HTML `<picture>` and `<video>` tags in the `.razor` files were successfully updated. However, the stylesheet `.less` files were overlooked. Since the heavy local media assets were deleted from the `wwwroot` directory, any styles relying on local paths (e.g., `background-image: url('../img/...')`) are currently resulting in 404 errors. These paths must be updated to the absolute Blob Storage URL. The logo is not found even in the html and razor files, so further investigation required regarding the logo image file. The encoding problem should be resolved,
            
            ### Acceptance Criteria (AC)
            Given a visitor on the landing page,
            When a section utilizing a CSS background image (e.g., hero section, parallax backgrounds) is rendered,
            Then the image loads successfully directly from the Azure Blob CDN without any 404 Not Found errors in the browser console,
            And all of the text is readable..
            
            Given the frontend codebase,
            When inspecting the compiled `.css` and source `.less` files,
            Then no local relative paths exist for the migrated heavy media assets.
            
            ### Definition of Done (DoD)
            - [ ] Do a global search in all `.less` files for `url(` or `url(`.
            - [ ] Replace relative local paths with the absolute base URL: `https://heiwasemedia.blob.core.windows.net/public-media/`.
            - [ ] Recompile the `.less` files into the final `.css` file.
            - [ ] Test the UI locally to ensure no broken backgrounds or missing icons occur.
            - [x] Make text readable
            - [ ] Commit and push the changes to the `master` branch.
            ```

   14. **[#110 — Manually Offload Large Media to Azure Blob Storage](https://github.com/zzsoltg/HeiwaseWeb/issues/110)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a DevOps engineer, I want to manually offload large static media files to an Azure Blob Storage container so that the application payload drops below the 250 MB limit and the CI/CD pipeline can successfully deploy to Azure Static Web Apps.
         
         ### Description
         Bypass the Azure Static Web Apps 250 MB deployment limit by removing heavy media assets (e.g., the large `hero.mp4` video) from the Git repository and the Blazor `wwwroot` folder. Provision the permanent Azure Storage Account via the Azure Portal early, create a container with anonymous read access, and manually upload the heavy assets. Finally, update the HTML/Razor components to reference the new absolute Blob URLs. This solves the immediate deployment blocker while establishing the exact storage resource that will be utilized by the API in later milestones.
         
         ### Acceptance Criteria (AC)
         Given the Azure Portal,
         When the Storage Account and public container are provisioned,
         Then the uploaded media files are accessible via a direct HTTPS URL in the browser.
         
         Given the Blazor application running in production,
         When a user navigates to the landing page,
         Then the hero video streams directly from the Azure Blob Storage CDN without any CORS or access errors.
         
         Given the local Git repository,
         When inspecting the total size of the compiled `wwwroot` folder,
         Then it is well below the 250 MB threshold (e.g., < 50 MB), ensuring a fast and stable SWA deployment.
         
         ### Definition of Done (DoD)
         - [ ] Azure Storage Account (LRS redundancy) is created in the Azure Portal.
         - [ ] A container (e.g., `public-media`) is created and set to "Blob (anonymous read access for blobs only)".
         - [ ] Large media files are manually uploaded to the container.
         - [ ] `<video>` and `<img>` tags in the `.razor` files are updated with the absolute Blob URLs.
         - [ ] The heavy media files are permanently deleted from the Git repository and `wwwroot`.
         - [ ] The GitHub Actions deployment runs successfully (green state).
         ```

   15. **[#130 — Fixing the heading color at the Social Media section](https://github.com/zzsoltg/HeiwaseWeb/issues/130)** — `CLOSED`
      - **Labels:** bug
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Club Owner, I want to change the color of the Social Media secition's heading, so the users can see a more unified design on the page.
         
         ### Description
         Define the common heading color of the page by examining other section's titles. After that modify the color to the same type at the social section's heading.
         
         ### AC
         Given the webpage opened in the browser,
         When scrolling to Social Media Section,
         Then the "Follow Us" heading's color matches the other heading's.
         
         ### DoD
         
         - [ ] The heading color is the same at all of the sections.
         - [ ] No regression caused at other parts of the project.
         ```

3. **M3: Azure Cloud Foundation & Solution Re-Architecture**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 16 (including sub-issues; 13 open, 3 closed)

   1. **[#38 — Multi-Project Solution Restructuring](https://github.com/zzsoltg/HeiwaseWeb/issues/38)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Software Architect, I want to restructure the current single-project solution into a 5-module Clean Architecture workspace so that responsibilities (UI, API, Data Access, Shared Contracts) are strictly separated and deployable to Azure.
         
         ### Description
         The current `Heiwase.App` solution consists of only one Blazor WASM project. This epic covers scaffolding the new required projects (`BlazorAdmin`, `Shared`, `Api`, `Infrastructure`) and setting up the correct dependency graph without circular references. It explicitly abandons the previous Firebase approach and prepares the ground for Azure Functions and Cosmos DB.
         
         ### Acceptance Criteria (AC)
         Given a clean branch
         When the developer opens the .slnx file
         Then exactly 5 specific projects (Blazor, BlazorAdmin, Shared, Api, Infrastructure) are visible and load successfully in Visual Studio.
         
         ### Definition of Done (DoD)
         
         - [ ] All 5 projects are created and target the correct framework (.NET 10).
         - [ ] Project dependencies are correctly configured.
         - [ ] The solution builds successfully without errors.
         - [ ] Code is reviewed and merged into the master branch.
         
         ### Dependencies and Constraints
         Must be completed before any Azure resource provisioning or backend logic can be implemented.
         ```

      1. **[#69 — Scaffold Heiwase.App.BlazorAdmin Project](https://github.com/zzsoltg/HeiwaseWeb/issues/69)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to scaffold the `Heiwase.App.BlazorAdmin` project so that the CMS dashboard has its own dedicated, independently deployable frontend.
            
            ### Description
            Create a new Blazor WebAssembly Standalone project targeting .NET 10. This will serve as the restricted content management system for club administrators.
            
            ### Acceptance Criteria (AC)
            Given the solution explorer,
            When I inspect Heiwase.App.BlazorAdmin,
            Then it is a Blazor WebAssembly Standalone app targeting net10.0.
            
            Given the build process,
            When I run dotnet build,
            Then the admin project builds independently of the main public Blazor app.
            
            ### Definition of Done (DoD)
            
            - [ ] Project is scaffolded.
            - [ ] Boilerplate code (weather forecast, default counter) is cleaned up.
            - [ ] Project is added to the .slnx file.
            ```

      2. **[#70 — Scaffold Heiwase.App.Shared & Define Core DTOs](https://github.com/zzsoltg/HeiwaseWeb/issues/70)** — `CLOSED`
         - **Labels:** none
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to create a `Heiwase.App.Shared` class library so that both frontends and the Azure backend can use a single source of truth for domain models and data transfer objects.
            
            ### Description
            Create a standard .NET 10 Class Library. Migrate existing data models (e.g., `HallOfFameMember`) from the public Blazor app into this shared project, and create new essential records (like `ArticleDto`).
            
            ### Acceptance Criteria (AC)
            Given the Heiwase.App.Shared project,
            When inspected,
            Then it contains no UI or database dependencies (pure C#).
            
            Given the public Blazor project,
            When it fetches the Hall of Fame,
            Then it uses the model referenced from the Shared library.
            
            ### Definition of Done (DoD)
            
            - [ ] All 5 projects are created and target the correct framework (.NET 10).
            - [ ] Project dependencies are correctly configured.
            - [ ] The solution builds successfully without errors.
            - [ ] Code is reviewed and merged into the master branch.
            
            ### Relevant Data Definitions
            
            - `ArticleDto `(Id, Title, MarkdownContent, CreatedAt, IsPublished)
            - `ApplicationFormDto `(ApplicantName, Email, Phone, IsMinor, GuardianName, SelectedCourse)
            ```

      3. **[#71 — Scaffold Heiwase.App.Api (Azure Functions v4)](https://github.com/zzsoltg/HeiwaseWeb/issues/71)** — `CLOSED`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to initialize the `Heiwase.App.Api` project as an Azure Functions Isolated Worker so that the application has a serverless backend capable of handling HTTP API requests.
            
            ### Description
            This project replaces the previously planned Firebase REST integration. It must be an Azure Functions v4 project using the .NET Isolated Worker model, which will act as the HTTP backend for both Blazor WASM applications.
            
            ### Acceptance Criteria (AC)
            Given the local development environment,
            When I press F5 on the Api project,
            Then the Azure Functions Core Tools CLI starts successfully on a local port.
            
            ### Definition of Done (DoD)
            
            - [x] Azure Functions v4 (Isolated Worker) project is created targeting net10.0.
            - [x] Project is added to the solution.
            - [x] A dummy HTTP trigger function (e.g., HealthCheck) is added to verify execution.
            ```

      4. **[#72 — Scaffold Heiwase.App.Infrastructure](https://github.com/zzsoltg/HeiwaseWeb/issues/72)** — `CLOSED`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to set up the `Heiwase.App.Infrastructure` class library so that all database (Cosmos DB) and storage (Blob) operational logic is isolated from the API and frontend layers.
            
            ### Description
            Scaffold a .NET 10 class library. This layer will eventually hold the `Microsoft.Azure.Cosmos` SDK and Blob Storage SDK implementations. Crucially, it will only be referenced by the `Api` project, never by the WASM clients.
            
            ### Acceptance Criteria (AC)
            Given the Infrastructure project,
            When checking its dependencies,
            Then it references Heiwase.App.Shared but has no references to any UI or API projects.
            
            ### Definition of Done (DoD)
            
            - [x] Project is scaffolded and added to the solution.
            - [x] Empty repository interfaces (e.g., IArticleRepository) are defined, and empty implementation classes are created in Infrastructure.
            ```

      5. **[#73 — Configure Solution Dependency Graph](https://github.com/zzsoltg/HeiwaseWeb/issues/73)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to configure the project reference graph so that Clean Architecture dependency rules are enforced and circular dependencies are avoided.
            
            ### Description
            Wire up the dependencies between the newly created 5 projects using `ProjectReference` in the `.csproj` files.
            
            ### Acceptance Criteria (AC)
            Given the API project,
            When it compiles,
            Then it successfully references Shared and Infrastructure.
            
            Given the WASM projects (Blazor and BlazorAdmin),
            When they compile,
            Then they ONLY reference the Shared project.
            
            Given the entire solution,
            When running dotnet build,
            Then the build succeeds without circular dependency warnings.
            
            ### Definition of Done (DoD)
            
            - [ ] All .csproj files are updated with correct <ProjectReference> tags.
            - [ ] The dotnet build command executes with 0 errors and 0 warnings related to project references.
            
            ### Business Rules / Architecture Constraints
            
            - `Blazor `→ `Shared`
            - `BlazorAdmin `→ `Shared`
            - `Infrastructure `→ `Shared`
            - `Api `→ `Shared`, `Infrastructure`
            - `Shared `→ No dependencies
            ```

   2. **[#74 — Azure Landing Zone & Data Platform Provisioning](https://github.com/zzsoltg/HeiwaseWeb/issues/74)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a DevOps engineer, I want to provision the Azure infrastructure (Cosmos DB, Blob Storage, Key Vault) and implement the data access layer in code so that the application has a secure, serverless foundation.
         
         ### Description
         This epic covers creating the Azure resources (Cosmos DB for NoSQL, Blob Storage for media), setting up their containers/partitions, and implementing the `Microsoft.Azure.Cosmos` SDK repository layer within the `Heiwase.App.Infrastructure` project.
         
         ### Acceptance Criteria (AC)
         Given the Azure Portal,
         When navigating to the production Resource Group,
         Then Cosmos DB, Storage Account, and Application Insights are provisioned and active.
         
         Given the Api project,
         When it requests data operations,
         Then it successfully communicates with Cosmos DB via the Infrastructure layer.
         
         ### Definition of Done (DoD)
         - [ ] Azure resources are provisioned on Free Tier / Consumption plans.
         - [ ] Database containers and partition keys are configured.
         - [ ] Repository interfaces and Cosmos DB implementations are completed in code.
         - [ ] Existing JSON data is migrated to the new database.
         ```

      1. **[#75 — Provision Azure Resources via Bicep (or Portal)](https://github.com/zzsoltg/HeiwaseWeb/issues/75)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a DevOps engineer, I want IaC templates (Bicep) for Resource Group, Cosmos DB, Storage Account, Key Vault, and Application Insights, so environments are reproducible and low-cost by default.
            
            ### Description
            Author Bicep modules (or perform structured manual setup) for the core Azure services. Cosmos DB must use the Free Tier (or Serverless) capacity mode, and the Storage Account should use LRS to minimize costs.
            
            ### Acceptance Criteria (AC)
            Given the deployment script,
            When executed against the Azure subscription,
            Then it provisions a Resource Group containing a Cosmos DB account, Storage Account, and Application Insights.
            
            ### Definition of Done (DoD)
            - [ ] Bicep files are created and committed to the repository (e.g., in an `infra/` folder).
            - [ ] Resources are successfully deployed to the `West Europe` region.
            - [ ] Cosmos DB Free Tier discount is successfully applied.
            ```

      2. **[#76 — Design Cosmos DB Containers & Partition Strategy](https://github.com/zzsoltg/HeiwaseWeb/issues/76)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a Data Architect, I want to design the containers and partition keys for Cosmos DB so that queries for articles and applications are efficient and cost-effective.
            
            ### Description
            Set up the NoSQL database schema and containers. Define partition keys (e.g., `/type` or `/year`) and indexing policies for the `articles`, `hallOfFame`, and `applications` containers.
            
            ### Acceptance Criteria (AC)
            Given the Cosmos DB Data Explorer,
            When inspecting the database,
            Then the required containers exist with correctly assigned partition keys.
            
            ### Definition of Done (DoD)
            - [ ] Database `HeiwaseDb` is created.
            - [ ] `articles` container is created.
            - [ ] `hallOfFame` container is created.
            - [ ] `applications` container is created.
            ```

      3. **[#77 — Implement the Cosmos DB Repository Layer](https://github.com/zzsoltg/HeiwaseWeb/issues/77)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a backend developer, I want to implement the Cosmos DB repository layer in the `Infrastructure` project so that the API can perform CRUD operations on the database securely.
            
            ### Description
            Implement the `IArticleRepository`, `IApplicationRepository`, and `IHallOfFameRepository` interfaces in the `Heiwase.App.Infrastructure` project using the `Microsoft.Azure.Cosmos` SDK. Use Managed Identity or Key Vault for connection string security.
            
            ### Acceptance Criteria (AC)
            Given the CosmosArticleRepository class,
            When the GetArticlesAsync method is called,
            Then it returns a list of ArticleDto objects retrieved from Cosmos DB via LINQ queries.
            
            ### Definition of Done (DoD)
            - [ ] `Microsoft.Azure.Cosmos` NuGet package is installed in the Infrastructure project.
            - [ ] Repository implementations for all 3 main entities are completed.
            - [ ] Dependency Injection is configured in the Api project's `Program.cs`.
            ```

      4. **[#78 — Implement Blob Storage Upload Service](https://github.com/zzsoltg/HeiwaseWeb/issues/78)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a backend developer, I want to implement a Blob Storage upload service so that images and media files can be uploaded from the CMS and served via a public CDN URL.
            
            ### Description
            Create an `IMediaService` in the `Infrastructure` project utilizing the `Azure.Storage.Blobs` SDK. It should accept a file stream, upload it to an Azure Storage container, and return the public URL.
            
            ### Acceptance Criteria (AC)
            Given a valid image stream,
            When the UploadImageAsync method is called,
            Then the file is saved to Azure Blob Storage and a valid HTTPS URL is returned.
            
            ### Definition of Done (DoD)
            - [ ] `Azure.Storage.Blobs` NuGet package is installed.
            - [ ] Upload service is implemented and registered in DI.
            - [ ] The target Blob container is configured for "Blob (anonymous read access for blobs only)".
            ```

      5. **[#79 — Seed Existing Data into Cosmos DB](https://github.com/zzsoltg/HeiwaseWeb/issues/79)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to seed the existing static JSON data into Cosmos DB so that the new dynamic API can serve the historical Hall of Fame content.
            
            ### Description
            Perform a one-time migration of `halloffame.json` and `hallOfFameEn.json` from the current Blazor project's `wwwroot/data` folder into the newly created `hallOfFame` Cosmos DB container.
            
            ### Acceptance Criteria (AC)
            Given the Cosmos DB container,
            When queried for Hall of Fame entries,
            Then all legacy records from the JSON files are present as NoSQL documents.
            
            ### Definition of Done (DoD)
            - [ ] Migration script/console app is written and executed.
            - [ ] Legacy JSON files are deleted from the `Heiwase.App.Blazor` repository.
            ```

   3. **[#80 — Identity, Secrets & Security Baseline](https://github.com/zzsoltg/HeiwaseWeb/issues/80)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Security Officer, I want to configure Microsoft Entra ID and API authorization so that only authenticated administrators can access the CMS and modify database records.
         
         ### Description
         This epic  covers setting up Microsoft Entra ID (App Registration), configuring MSAL.NET in the Admin Blazor app, and securing the Azure Functions API using JWT bearer token validation.
         
         ### Acceptance Criteria (AC)
         Given an anonymous user,
         When they attempt to access an HTTP POST endpoint on the API,
         Then the server returns a 401 Unauthorized response.
         
         Given an authenticated admin,
         When they provide a valid Entra ID token,
         Then the API accepts the request and performs the operation.
         
         ### Definition of Done (DoD)
         - [ ] Microsoft Entra ID App Registration is configured.
         - [ ] Admin Blazor app successfully acquires and caches JWT tokens.
         - [ ] Azure Functions API validates tokens before executing write operations.
         ```

      1. **[#81 — Register Microsoft Entra ID App](https://github.com/zzsoltg/HeiwaseWeb/issues/81)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As an infrastructure admin, I want to register an application in Microsoft Entra ID so that it can act as the Identity Provider for the Blazor Admin CMS.
            
            ### Description
            Create an App Registration in the Azure Portal. Configure redirect URIs for the local development environment and the production Azure Static Web Apps URL. Define an "Administrator" app role.
            
            ### Acceptance Criteria (AC)
            Given the Azure Portal,
            When viewing Microsoft Entra ID App Registrations,
            Then a "Heiwase Admin CMS" application exists with the correct SPA redirect URIs.
            
            ### Definition of Done (DoD)
            - [ ] App Registration is created.
            - [ ] SPA authentication platform is added with correct URIs.
            - [ ] "Administrator" App Role is defined in the manifest.
            ```

      2. **[#82 — Implement Admin Login via MSAL.NET](https://github.com/zzsoltg/HeiwaseWeb/issues/82)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a club administrator, I want to log in to the CMS using my Microsoft account so that I can access the restricted content management features.
            
            ### Description
            Install `Microsoft.Authentication.WebAssembly.Msal` in the `Heiwase.App.BlazorAdmin` project. Configure the authentication provider in `Program.cs` using the Client ID from the Entra ID App Registration. Create a Login/Logout UI component.
            
            ### Acceptance Criteria (AC)
            Given an unauthenticated state on the Admin app,
            When the user clicks "Login",
            Then they are redirected to the Microsoft login screen and returned authenticated.
            
            ### Definition of Done (DoD)
            - [ ] MSAL NuGet package is installed.
            - [ ] `appsettings.json` is configured with TenantId and ClientId.
            - [ ] Login and Logout buttons are functional.
            ```

      3. **[#83 — Enforce Authorization on Azure Functions API](https://github.com/zzsoltg/HeiwaseWeb/issues/83)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a backend developer, I want to protect the API endpoints with JWT validation so that malicious actors cannot modify the database.
            
            ### Description
            Configure the Azure Functions Isolated Worker middleware to validate JWT Bearer tokens issued by Microsoft Entra ID. Apply `[Authorize]`-like logic to all POST, PUT, and DELETE HTTP triggers, ensuring only users with the "Administrator" role can execute them.
            
            ### Acceptance Criteria (AC)
            Given a POST request to `/api/articles` without an Authorization header,
            When the request reaches the Azure Function,
            Then it is immediately rejected with a 401 status code.
            
            ### Definition of Done (DoD)
            - [ ] JWT validation middleware is implemented in the `Api` project.
            - [ ] Read endpoints (GET) remain anonymous.
            - [ ] Write endpoints (POST/PUT/DELETE) enforce authentication.
            ```

4. **M4: Public Site: Dynamic Content on Azure**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 10 (including sub-issues; 10 open, 0 closed)

   1. **[#84 — Content API Surface (Azure Functions)](https://github.com/zzsoltg/HeiwaseWeb/issues/84)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a frontend application, I want to retrieve and submit data via HTTP endpoints so that the public site can display dynamic content and process user forms securely.
         
         ### Description
         Build the read-only HTTP triggers for public content (Articles, Hall of Fame) and the write endpoint for the application form inside the `Heiwase.App.Api` project. This epic also introduces Azure Communication Services to handle email notifications, replacing the external Formspree dependency.
         
         ### Acceptance Criteria (AC)
         Given a GET request to the `/api/articles` endpoint,
         When executed by an anonymous user,
         Then it returns a JSON array of published `ArticleDto`s with a 200 OK status.
         
         ### Definition of Done (DoD)
         - [ ] Read-only endpoints for Articles and Hall of Fame are deployed and accessible.
         - [ ] Write endpoint for applications is functional and integrated with Azure Communication Services.
         - [ ] Endpoints utilize the `Infrastructure` repository layer.
         - [ ] Code is reviewed and merged into the `master` branch.
         ```

      1. **[#85 — Expose Articles Endpoints](https://github.com/zzsoltg/HeiwaseWeb/issues/85)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a public user, I want to fetch news articles from the API so that I can read the latest updates.
            
            ### Description
            Create `GET /api/articles` (returns a list of published articles) and `GET /api/articles/{id}` (returns a single article) endpoints in the Azure Functions project.
            
            ### Acceptance Criteria (AC)
            Given a valid article ID,
            When a GET request is sent to `/api/articles/{id}`,
            Then the API returns a 200 OK with the requested `ArticleDto`.
            
            Given a non-existent article ID,
            When a GET request is sent,
            Then the API returns a 404 Not Found.
            
            ### Definition of Done (DoD)
            - [ ] Both list and detail endpoints are implemented.
            - [ ] Queries are filtered to return only articles where `IsPublished == true`.
            ```

      2. **[#86 — Expose the Hall of Fame Endpoint](https://github.com/zzsoltg/HeiwaseWeb/issues/86)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a public user, I want to fetch the Hall of Fame data from the API so that I can see the current champions and senpais.
            
            ### Description
            Create a `GET /api/halloffame` endpoint that queries the Cosmos DB `hallOfFame` container and returns a list of `HallOfFameMemberDto`s.
            
            ### Acceptance Criteria (AC)
            Given the API is running,
            When a GET request is sent to `/api/halloffame`,
            Then the API returns a 200 OK with the list of members sorted by rank or achievement.
            
            ### Definition of Done (DoD)
            - [ ] Endpoint is implemented.
            - [ ] Uses the `IHallOfFameRepository` to fetch data.
            ```

      3. **[#87 — Expose Application Submission Endpoint & Email Trigger](https://github.com/zzsoltg/HeiwaseWeb/issues/87)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a prospective member, I want to submit my application securely so that the club administrators are notified immediately.
            
            ### Description
            Create a `POST /api/applications` endpoint. This endpoint must validate the incoming `ApplicationFormDto` (preserving the server-side guardian-for-minors rule), save the record to Cosmos DB, and trigger an email notification to the club admins using Azure Communication Services Email.
            
            ### Acceptance Criteria (AC)
            Given an applicant who is a minor,
            When they submit the form without a `GuardianName`,
            Then the API returns a 400 Bad Request with a validation error message.
            
            Given a valid application,
            When submitted,
            Then the record is saved to the database and an email is triggered via Azure Communication Services.
            
            ### Definition of Done (DoD)
            - [ ] Endpoint is implemented.
            - [ ] Business logic validation is executed on the server side.
            - [ ] Azure Communication Services Email SDK is integrated and functional.
            ```

   2. **[#88 — Public Site Consumption of the Azure API](https://github.com/zzsoltg/HeiwaseWeb/issues/88)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a site visitor, I want to browse news, view the Hall of Fame, and submit applications directly on the website so that I have a fast, interactive, and up-to-date experience.
         
         ### Description
         Update the `Heiwase.App.Blazor` standalone project to drop static JSON files and the Formspree integration. Wire the UI components (`HallOfFameSection`, `ContactSection`, and News views) to consume the new `Heiwase.App.Api` endpoints using `HttpClient`.
         
         ### Acceptance Criteria (AC)
         Given the public landing page,
         When the Hall of Fame section scrolls into view,
         Then it fetches data dynamically from the Azure API instead of local static files.
         
         ### Definition of Done (DoD)
         - [ ] All static data dependencies are removed from the Blazor project.
         - [ ] Loading and error states are implemented for all asynchronous API calls.
         - [ ] Markdown content is safely rendered as HTML.
         ```

      1. **[#89 — Build Public Articles/News Views](https://github.com/zzsoltg/HeiwaseWeb/issues/89)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a site visitor, I want to see a list of news cards and click on them to read the full story so that I can stay informed about club events.
            
            ### Description
            Implement a new page component (a section in `Home.razor`) that calls `GET /api/articles`. Implement a detail view route (e.g., `/news/{id}`) to display a specific article.
            
            ### Acceptance Criteria (AC)
            Given the news feed,
            When the user clicks on an article card,
            Then the application navigates to the article detail view and fetches the full content.
            
            ### Definition of Done (DoD)
            - [ ] Listing UI is implemented.
            - [ ] Detail UI is implemented with routing.
            - [ ] Loading spinners are displayed during HTTP requests.
            ```

      2. **[#90 — Render Markdown Articles Securely](https://github.com/zzsoltg/HeiwaseWeb/issues/90)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a reader, I want the articles to be beautifully formatted with headings, images, and paragraphs so that they are easy to read.
            
            ### Description
            Integrate the `Markdig` (with `UseAdvancedExtensions`) NuGet package in the `Heiwase.App.Blazor` project. Convert the raw `MarkdownContent` string from the API into HTML, and render it safely using `MarkupString`. Implement sanitization to prevent XSS attacks.
            
            ### Acceptance Criteria (AC)
            Given an article containing Markdown (`**bold**`),
            When rendered on the detail page,
            Then it displays as formatted HTML (`<strong>bold</strong>`) without executing any injected JavaScript.
            
            ### Definition of Done (DoD)
            - [ ] Markdig package is installed.
            - [ ] Markdown-to-HTML conversion pipeline is configured (preferably with a sanitization step or strict pipeline settings).
            - [ ] The LESS design system is extended to style the generated HTML elements.
            ```

      3. **[#91 — Connect Hall of Fame Section to the Azure API](https://github.com/zzsoltg/HeiwaseWeb/issues/91)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a site visitor, I want to see the Hall of Fame slider populated with real-time data so that I can view the latest achievements.
            
            ### Description
            Refactor `HallOfFameSection.razor.cs`. Remove the `HttpClient.GetFromJsonAsync` call pointing to `wwwroot/data/halloffame.json` and replace it with a call to the Azure API `/api/halloffame`. Add proper loading placeholders.
            
            ### Acceptance Criteria (AC)
            Given a slow network connection,
            When the Hall of Fame section loads,
            Then a loading spinner is shown until the data arrives from the API.
            
            ### Definition of Done (DoD)
            - [ ] API integration is complete.
            - [ ] Static JSON files are fully removed from `wwwroot/data`.
            - [ ] JS interop slider animation still functions correctly with dynamic data.
            ```

      4. **[#92 — Migrate Contact Form to the Azure Endpoint](https://github.com/zzsoltg/HeiwaseWeb/issues/92)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As an applicant, I want to submit my form and see a success notification so that I know my application was received.
            
            ### Description
            Refactor `ContactSection.razor.cs`. Replace the hardcoded Formspree URL with the new `/api/applications` endpoint. Ensure the existing `ApplicantModel` validation rules trigger before submission, and keep the existing success/error toastr notification behavior.
            
            ### Acceptance Criteria (AC)
            Given a filled-out contact form,
            When the user clicks submit,
            Then a POST request is sent to the Azure API, and upon a 200 OK response, a success toast is shown and the form resets.
            
            ### Definition of Done (DoD)
            - [ ] Formspree endpoint is completely removed.
            - [ ] HttpClient posts `ApplicationFormDto` to Azure API.
            - [ ] UI correctly handles both successful submissions and HTTP 400 validation errors from the server.
            ```

      5. **[#118 — Implement Dynamic SEO Metadata & Open Graph Tags for Public Pages](https://github.com/zzsoltg/HeiwaseWeb/issues/118)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a marketing-conscious club leader, I want each public page and news article to expose accurate, dynamic SEO metadata so that search engines rank the site well and shared links display rich previews on social media.
            
            ### Description
            Extend `Heiwase.App.Blazor` with dynamic `<title>`, meta description, canonical URL, and Open Graph/Twitter Card tags for the landing page and each article detail route (`/news/{id}`). Use `<PageTitle>`/`<HeadContent>` to set these per route, sourcing content from the `ArticleDto` returned by `/api/articles/{id}` (#85). Add a static `robots.txt` and a dynamically generated `sitemap.xml` covering published articles.
            
            ### Acceptance Criteria (AC)
            Given a visitor navigates to `/news/{id}` for a published article,
            When the page finishes rendering,
            Then the `<title>`, meta description, and Open Graph image reflect that article's title, summary, and cover image.
            
            Given a search engine crawler requests `/sitemap.xml`,
            When the site responds,
            Then it lists the home page and every published article URL with a last-modified date.
            
            Given a visitor shares a news article link on social media,
            When the link preview is generated,
            Then the Open Graph title/description/image display correctly instead of blank placeholders.
            
            ### Definition of Done (DoD)
            - [ ] `<PageTitle>` and dynamic meta tags implemented for landing page and article detail routes.
            - [ ] Open Graph and Twitter Card tags populated per-article.
            - [ ] `robots.txt` added to `wwwroot`.
            - [ ] `sitemap.xml` generation implemented and accessible.
            ```

5. **M5: Admin Portal: Secure Content Management System**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 8 (including sub-issues; 8 open, 0 closed)

   1. **[#93 — Admin Shell & Authentication](https://github.com/zzsoltg/HeiwaseWeb/issues/93)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a club administrator, I want a secure dashboard shell so that I can navigate between different management modules while ensuring unauthorized visitors cannot access the system.
         
         ### Description
         Build the foundational layout and route guarding for the `Heiwase.App.BlazorAdmin` project. Unauthenticated users must be redirected to the Microsoft Entra ID sign-in page. The authenticated shell must expose a navigation menu to access News, Hall of Fame, and Applications.
         
         ### Acceptance Criteria (AC)
         Given an unauthenticated user,
         When they attempt to navigate to any admin route (e.g., `/news-manager`),
         Then they are automatically redirected to the sign-in page.
         
         Given an authenticated admin,
         When they log in successfully,
         Then they see the main dashboard layout with a functional sidebar navigation.
         
         ### Definition of Done (DoD)
         - [ ] Blazor Router is wrapped in `<CascadingAuthenticationState>` and `<AuthorizeRouteView>`.
         - [ ] Admin layout (`MainLayout.razor`) is implemented using Radzen components.
         - [ ] Sidebar/navigation menu is fully functional.
         ```

      1. **[#94 — List and Delete Published News](https://github.com/zzsoltg/HeiwaseWeb/issues/94)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a content manager, I want to see a list of all articles and be able to delete outdated ones so that I can manage the club's news feed.
            
            ### Description
            Create a `RadzenDataGrid` to list all articles fetched from the `/api/articles` endpoint. Include a delete button in the grid that triggers a confirmation dialog before sending a DELETE request to the API.
            
            ### Acceptance Criteria (AC)
            Given the article list,
            When the admin clicks the "Delete" button,
            Then a confirmation modal appears before the API call is made.
            
            ### Definition of Done (DoD)
            - [ ] Data grid is populated via `HttpClient.GetFromJsonAsync`.
            - [ ] Delete functionality is implemented with a confirmation dialog.
            - [ ] Success/error toast notifications are displayed.
            ```

      2. **[#95 — Create/Edit News Form & Markdown Editor](https://github.com/zzsoltg/HeiwaseWeb/issues/95)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a content manager, I want to write and format news using a text editor so that I don't have to write raw Markdown syntax manually.
            
            ### Description
            Build the article form containing a title input, a date picker, and a Markdown editor text area. Add a WYSIWYG toolbar (Bold, Italic, Link, Heading, Quote) over the text area utilizing JS interop to insert the correct Markdown syntax at the cursor position.
            
            ### Acceptance Criteria (AC)
            Given the Markdown editor,
            When the user highlights text and clicks the "Bold" toolbar button,
            Then the text is wrapped in `**` syntax.
            
            ### Definition of Done (DoD)
            - [ ] Form is bound to the `ArticleDto` model.
            - [ ] JS interop functions for cursor manipulation and text insertion are written.
            - [ ] Toolbar buttons correctly apply Markdown formatting.
            ```

      3. **[#96 — Upload Images from Editor to Blob Storage](https://github.com/zzsoltg/HeiwaseWeb/issues/96)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a content manager, I want to insert images directly into my articles so that the news feed is visually engaging.
            
            ### Description
            Wire an "Insert Image" button in the WYSIWYG toolbar to a file picker. When a file is selected, upload it via the API's Blob Storage upload service. Once the upload is complete, automatically insert the returned CDN URL as Markdown image syntax (`![alt](url)`) at the cursor position.
            
            ### Acceptance Criteria (AC)
            Given the editor,
            When an image is successfully uploaded,
            Then the Markdown syntax `![image](https://...blob.core.windows.net/...)` is inserted into the text area.
            
            ### Definition of Done (DoD)
            - [ ] File picker is integrated into the editor toolbar.
            - [ ] Upload progress and error states are handled in the UI.
            - [ ] The returned URL is successfully appended to the Markdown content.
            ```

   2. **[#97 — Hall of Fame & Application Management](https://github.com/zzsoltg/HeiwaseWeb/issues/97)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a club administrator, I want to manage Hall of Fame members and review incoming training applications so that the club's records are accurate and new leads are processed promptly.
         
         ### Description
         Build the management interfaces for the `hallOfFame` and `applications` data. The Hall of Fame requires full CRUD capabilities. The Applications view requires listing, filtering (e.g., reviewed vs. unreviewed), and safe viewing of sensitive PII (especially minor/guardian details), reflecting the asymmetric access rules established in Epic 2.3.
         
         ### Acceptance Criteria (AC)
         Given the applications management view,
         When the admin selects an application,
         Then they can view the full details (including Guardian Name if applicable) and mark the application as "Reviewed".
         
         ### Definition of Done (DoD)
         - [ ] Hall of Fame CRUD views are built and wired to the API.
         - [ ] Applications list and detail views are built.
         - [ ] Filtering (e.g., by course or review status) is implemented using Radzen DataGrid.
         ```

   3. **[#119 — Progressive Web App (PWA) Support for the Admin CMS](https://github.com/zzsoltg/HeiwaseWeb/issues/119)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a club administrator, I want the CMS to behave like an installable app with fast reloads and partial offline access so that I can manage content efficiently even on unreliable connections.
         
         ### Description
         Convert `Heiwase.App.BlazorAdmin` into a Progressive Web App, per the thesis's January goal of "Integrating PWA features, Service Worker, and offline cache." Covers the web app manifest, icon set, service worker registration, and an asset-caching/offline-fallback strategy using standard Blazor WASM PWA conventions.
         
         ### Acceptance Criteria (AC)
         Given a supported browser,
         When an administrator visits the BlazorAdmin site,
         Then the browser offers an "Install app"/"Add to Home Screen" prompt.
         
         Given the admin PWA was loaded once while online,
         When the administrator reopens it while offline,
         Then the app shell (login screen, cached static assets) still loads instead of a browser network-error page.
         
         ### Definition of Done (DoD)
         - [ ] `manifest.webmanifest` and app icons added to `Heiwase.App.BlazorAdmin`.
         - [ ] A registered service worker caches the app shell and static assets.
         - [ ] The app is installable and passes a Lighthouse PWA audit with no critical failures.
         - [ ] Code is reviewed and merged into `master`.
         ```

      1. **[#120 — Configure PWA Manifest, Icons & Service Worker Registration for BlazorAdmin](https://github.com/zzsoltg/HeiwaseWeb/issues/120)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a club administrator, I want the CMS to be installable on my device so that I can open it like a native app.
            
            ### Description
            Enable the Blazor WASM PWA feature for `Heiwase.App.BlazorAdmin`. Add `manifest.webmanifest` (name, theme color, multi-resolution icons) referenced from `index.html`, and register the auto-generated `service-worker.js`.
            
            ### Acceptance Criteria (AC)
            Given the published app,
            When `manifest.webmanifest` is inspected in DevTools "Application" tab,
            Then it declares `name`, `short_name`, `start_url`, `display: standalone`, and 192x192/512x512 icons.
            
            Given the deployed admin site,
            When a Chromium browser evaluates installability,
            Then no manifest or service-worker errors are reported.
            
            ### Definition of Done (DoD)
            - [ ] `manifest.webmanifest` and icon assets added to `wwwroot`.
            - [ ] `index.html` references the manifest and theme color.
            - [ ] Service worker registration is active in production builds.
            ```

      2. **[#121 — Implement Offline Asset Caching & Network-Fallback Strategy for the Admin PWA](https://github.com/zzsoltg/HeiwaseWeb/issues/121)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a club administrator, I want the CMS shell to remain usable when my connection briefly drops so that I don't lose in-progress work or see a broken page.
            
            ### Description
            Customize the generated service worker's caching strategy for `Heiwase.App.BlazorAdmin`: cache-first for the WASM runtime/DLLs/static assets, network-first with an offline fallback for API calls (live data cannot be meaningfully edited offline). Show an "You are offline" banner when connectivity is lost.
            
            ### Acceptance Criteria (AC)
            Given the administrator has loaded the CMS at least once,
            When their device loses connectivity,
            Then the app shell still renders and an offline banner is displayed.
            
            Given the administrator is offline and attempts to save an article,
            When the save fails due to no connectivity,
            Then a clear error message is shown and no data is silently lost.
            
            ### Definition of Done (DoD)
            - [ ] Cache-first strategy configured for static/WASM assets.
            - [ ] Network-first (with graceful failure messaging) configured for API calls.
            - [ ] Offline-status banner implemented using `online`/`offline` browser events.
            ```

6. **M6: Azure DevOps, Security, Observability & Go-Live**
   - **Milestone description:** Not available in the milestone metadata returned for this export.
   - **Issues:** 16 (including sub-issues; 16 open, 0 closed)

   1. **[#98 — CI/CD Automation](https://github.com/zzsoltg/HeiwaseWeb/issues/98)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a DevOps engineer, I want automated CI/CD pipelines for both frontends and the shared API so that code changes are seamlessly built, tested, and deployed to Azure Static Web Apps without manual intervention.
         
         ### Description
         Extend the existing GitHub Actions setup to handle the multi-project architecture. The public `Heiwase.App.Blazor` and `Heiwase.App.BlazorAdmin` apps need separate workflows deploying to their respective Azure Static Web Apps resources. `Heiwase.App.Api` must be linked as the shared API for both. Deployment authentication should be upgraded to GitHub OIDC. Per the thesis's DevOps/testing plan, the pipeline must also run the automated xUnit suite (see the new "Automated Testing & Quality Assurance" epic) as a mandatory quality gate: any run with failing tests must stop before the deploy step and must never publish a broken build.
         
         ### Acceptance Criteria (AC)
         Given a new pull request against `master`,
         When the workflow triggers,
         Then a temporary Preview Environment is provisioned for both public and admin sites.
         
         Given a merged pull request,
         When the workflow completes,
         Then production is updated automatically.
         
         Given a pull request that modifies `Api` or `Infrastructure`,
         When the workflow runs,
         Then `dotnet test` executes the full xUnit suite before build/deploy.
         
         Given a workflow run with failing xUnit tests,
         When evaluated,
         Then the job fails immediately, the Azure deploy step is skipped, and the PR check shows red/failed.
         
         ### Definition of Done (DoD)
         
         - [ ] CI/CD pipelines for both public and admin apps are fully automated via GitHub Actions.
         - [ ] The `Api` project is deployed and linked to both SWAs.
         - [ ] Workflows execute successfully (green) on `master`.
         - [ ] A `dotnet test` step runs before the deploy step in every workflow.
         - [ ] The deploy step is conditioned on the test step succeeding.
         ```

      1. **[#99 — Configure Dual SWA Workflows & Link API](https://github.com/zzsoltg/HeiwaseWeb/issues/99)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a developer, I want to configure the YAML workflows so that both the public and admin Blazor apps are deployed alongside the shared Azure Functions backend.
            
            ### Description
            Create or update the `.github/workflows/azure-static-web-apps-*.yml` files. Specify the correct `app_location` for each Blazor project and the `api_location` pointing to the `Heiwase.App.Api` project. Ensure the Azure Portal is configured to link this single API to both Static Web Apps.
            
            ### Acceptance Criteria (AC)
            Given the Azure Portal,
            When inspecting the APIs blade of either Static Web App,
            Then the `Heiwase.App.Api` Azure Function is listed as the linked backend.
            
            ### Definition of Done (DoD)
            - [ ] Workflow YAML files are updated with correct build paths.
            - [ ] API linking is configured in Azure.
            - [ ] End-to-end deployment succeeds.
            ```

      2. **[#100 — Migrate Deployment Auth to GitHub OIDC](https://github.com/zzsoltg/HeiwaseWeb/issues/100)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a security officer, I want to use OpenID Connect (OIDC) for deployment authentication so that we don't have to manage or rotate long-lived deployment tokens.
            
            ### Description
            Replace the standard Azure Static Web Apps deployment token secret with federated credentials using Microsoft Entra ID and GitHub OIDC. Update the GitHub Actions workflow to use `azure/login` with `client-id`, `tenant-id`, and `subscription-id`.
            
            ### Acceptance Criteria (AC)
            Given the GitHub repository secrets,
            When the deployment workflow runs,
            Then it authenticates to Azure securely without using a hardcoded `AZURE_STATIC_WEB_APPS_API_TOKEN`.
            
            ### Definition of Done (DoD)
            - [ ] Federated identity credential is created in Microsoft Entra ID for the GitHub repo.
            - [ ] GitHub Actions workflow is updated with `permissions: id-token: write`.
            - [ ] Deployment succeeds using OIDC.
            ```

      3. **[#101 — Observability & Cost Governance](https://github.com/zzsoltg/HeiwaseWeb/issues/101)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a system administrator, I want observability and cost controls in place so that I can monitor application health and guarantee the club incurs no unexpected cloud charges.
            
            ### Description
            Integrate Azure Application Insights into the `Api` project for performance and error tracking. Configure an Azure Cost Management budget with email alerts to proactively prevent billing surprises, ensuring the architecture remains within the Free Tier limits.
            
            ### Acceptance Criteria (AC)
            Given the Azure Portal Cost Management,
            When viewing the subscription,
            Then a strict budget alert is active and set near $0.
            
            ### Definition of Done (DoD)
            - [ ] Application Insights SDK is integrated into the API.
            - [ ] Budget alerts are configured.
            - [ ] 429 (Too Many Requests) throttling alerts for Cosmos DB are set up.
            ```

         1. **[#102 — Enable Application Insights for Telemetry](https://github.com/zzsoltg/HeiwaseWeb/issues/102)** — `OPEN`
            - **Labels:** enhancement
            - **Full description (verbatim issue body):**
               ```markdown
               ### User Story
               As a developer, I want to track API requests, errors, and latency so that I can quickly debug issues in production.
               
               ### Description
               Install the `Microsoft.ApplicationInsights.WorkerService` package in the `Api` project. Configure the connection string via Key Vault or Azure App Settings. Verify that Cosmos DB dependency calls are automatically tracked.
               
               ### Acceptance Criteria (AC)
               Given the Application Insights dashboard,
               When a user navigates the public site,
               Then HTTP requests and their corresponding Cosmos DB query latencies appear in the end-to-end transaction viewer.
               
               ### Definition of Done (DoD)
               - [ ] Telemetry package is installed and configured.
               - [ ] Logs and exceptions are successfully captured in Azure.
               ```

         2. **[#103 — Configure Azure Budget & Cost Alerts](https://github.com/zzsoltg/HeiwaseWeb/issues/103)** — `OPEN`
            - **Labels:** enhancement
            - **Full description (verbatim issue body):**
               ```markdown
               ### User Story
               As the club's financial stakeholder, I want to receive an email if cloud costs exceed 1 EUR so that I can react before a large bill is generated.
               
               ### Description
               Create a Budget in Azure Cost Management for the project's Resource Group. Set the threshold to a minimal amount (e.g., 1 EUR/USD) and configure the action group to send an email to the club's administrative email address.
               
               ### Acceptance Criteria (AC)
               Given the Azure Budget configuration,
               When forecasted or actual costs exceed the defined threshold,
               Then an automated email alert is immediately dispatched to the configured address.
               
               ### Definition of Done (DoD)
               - [ ] Budget is created in Azure Cost Management.
               - [ ] Alert condition (e.g., 100% of 1 EUR budget) is defined.
               - [ ] Action Group with the correct email address is attached.
               ```

   2. **[#104 — Custom Domain & Production Cutover](https://github.com/zzsoltg/HeiwaseWeb/issues/104)** — `OPEN`
      - **Labels:** enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a product owner, I want to map our official domains to the new Azure infrastructure and turn off old services so that the new site goes live to the public and legacy dependencies are removed.
         
         ### Description
         Finalize the migration by pointing the official club domain (e.g., `szegedkarate.hu`) to the public Azure Static Web App, and an admin subdomain (e.g., `admin.szegedkarate.hu`) to the Admin app. Once the DNS propagation is complete and monitored, safely decommission the Netlify site and the Formspree account.
         
         ### Acceptance Criteria (AC)
         Given a web browser,
         When a user navigates to the official club domain,
         Then the Azure-hosted Blazor WASM site is served securely over HTTPS.
         
         ### Definition of Done (DoD)
         - [ ] DNS records (CNAME/ALIAS) are updated.
         - [ ] Managed TLS/SSL certificates are successfully issued by Azure.
         - [ ] Legacy systems (Netlify, Formspree) are permanently deactivated.
         ```

      1. **[#105 — Configure Custom Domains & Managed TLS](https://github.com/zzsoltg/HeiwaseWeb/issues/105)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a site visitor, I want to access the site via its official domain name with a secure HTTPS connection so that I know the site is legitimate and safe.
            
            ### Description
            Add custom domains to both Azure Static Web Apps. Configure DNS records at the domain registrar. Ensure Azure successfully generates and binds the free managed TLS/SSL certificates for both the root domain and the admin subdomain.
            
            ### Acceptance Criteria (AC)
            Given the SSL checker tools,
            When validating the production domain,
            Then it returns a valid, automatically managed certificate with no browser warnings.
            
            ### Definition of Done (DoD)
            - [ ] Custom domain added to public SWA.
            - [ ] Custom subdomain added to Admin SWA.
            - [ ] TLS certificates are active for both domains.
            ```

      2. **[#106 — Decommission Netlify and Formspree](https://github.com/zzsoltg/HeiwaseWeb/issues/106)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a system administrator, I want to shut down legacy platforms after a successful cutover so that the architecture remains clean and no ghost services are running.
            
            ### Description
            After a 48-hour monitoring period post-DNS cutover, delete the original Netlify site configuration to prevent any stale builds. Close the Formspree account since Azure Communication Services now handles email delivery. Delete any related legacy secrets from the GitHub repository.
            
            ### Acceptance Criteria (AC)
            Given the Netlify dashboard,
            When searching for the club's project,
            Then the site is no longer listed or active.
            
            ### Definition of Done (DoD)
            - [ ] Netlify site is deleted.
            - [ ] Formspree endpoint is deleted.
            - [ ] Obsolete GitHub Secrets (if any) are removed.
            ```

   3. **[#122 — Automated Testing & Quality Assurance](https://github.com/zzsoltg/HeiwaseWeb/issues/122)** — `OPEN`
      - **Labels:** documentation, enhancement
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a Software Architect, I want an automated test suite covering the API and infrastructure layers so that regressions are caught before every deployment and the thesis's quality goals are demonstrably met.
         
         ### Description
         Covers the thesis's March goal of "Writing Unit tests" and "Integrating automated testing into the CI/CD pipeline." Introduces xUnit test projects for `Heiwase.App.Api` and `Heiwase.App.Infrastructure`, mocking Cosmos DB/Blob Storage dependencies, and connects results to the CI/CD quality gate defined in the updated #98.
         
         ### Acceptance Criteria (AC)
         Given the solution is built in CI,
         When `dotnet test` runs against the new test projects,
         Then all tests run without requiring a live Cosmos DB or Blob Storage connection.
         
         Given a developer introduces a regression (e.g., removes the minor/guardian validation rule),
         When the test suite runs,
         Then at least one existing unit test fails, clearly identifying the broken behavior.
         
         ### Definition of Done (DoD)
         - [ ] `Heiwase.App.Api.Tests` and `Heiwase.App.Infrastructure.Tests` xUnit projects created and added to the `.slnx`.
         - [ ] Test projects wired into the CI/CD pipeline per updated #98.
         - [ ] Code reviewed and merged into `master`.
         ```

      1. **[#123 — Write xUnit Unit Tests for Heiwase.App.Api Business Logic](https://github.com/zzsoltg/HeiwaseWeb/issues/123)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a backend developer, I want unit tests around the Azure Functions HTTP triggers so that I can refactor the API with confidence that existing behavior is preserved.
            
            ### Description
            Create `Heiwase.App.Api.Tests`. Using Moq (or a hand-written fake), mock `IArticleRepository`, `IHallOfFameRepository`, and `IApplicationRepository` (#77) to unit test the HTTP triggers in isolation: article listing/category filtering (#85), the 404 for missing articles, and the minor/guardian validation rule on the applications endpoint (#87).
            
            ### Acceptance Criteria (AC)
            Given a mocked `IArticleRepository` returning published and unpublished articles,
            When `GetArticles` executes,
            Then only published articles are returned.
            
            Given an `ApplicationFormDto` with `IsMinor = true` and no `GuardianName`,
            When `SubmitApplication` executes,
            Then it returns a 400 Bad Request and does not call the repository's save method.
            
            ### Definition of Done (DoD)
            - [ ] `Heiwase.App.Api.Tests` created with xUnit and Moq references.
            - [ ] At least one test class per HTTP trigger (Articles, Hall of Fame, Applications).
            - [ ] All tests pass locally via `dotnet test`.
            ```

      2. **[#124 — Write xUnit Unit Tests for the Cosmos DB & Blob Storage Repository Layer](https://github.com/zzsoltg/HeiwaseWeb/issues/124)** — `OPEN`
         - **Labels:** enhancement
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a backend developer, I want unit tests for the `Heiwase.App.Infrastructure` repositories so that data-access logic (query filters, partition key usage, mapping) is verified without a live Azure subscription.
            
            ### Description
            Create `Heiwase.App.Infrastructure.Tests`. Use the Cosmos DB emulator or a fake/testable abstraction to verify `CosmosArticleRepository`, `CosmosHallOfFameRepository`, and the Blob `IMediaService` (#78) build correct queries (including the `/category` filter from #76) and map documents to DTOs correctly.
            
            ### Acceptance Criteria (AC)
            Given a fake Cosmos container seeded with articles of all three categories,
            When `GetArticlesAsync(category: "Event")` is called,
            Then only `Event`-category articles are returned.
            
            Given a valid file stream passed to `IMediaService.UploadImageAsync`,
            When executed against a test/mock Blob container,
            Then it returns a well-formed HTTPS URL and the upload call is verified to have been invoked exactly once.
            
            ### Definition of Done (DoD)
            - [ ] `Heiwase.App.Infrastructure.Tests` created with xUnit references.
            - [ ] Repository tests run against the Cosmos DB emulator or a fake substitute.
            - [ ] All tests pass locally and in CI.
            ```

      3. **[#125 — Cross-Browser & Responsive UI Testing Across Devices and Screen Sizes](https://github.com/zzsoltg/HeiwaseWeb/issues/125)** — `OPEN`
         - **Labels:** documentation
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a Product Owner, I want the public site and admin CMS validated across major browsers and screen sizes so that every visitor and administrator gets a consistent, working experience.
            
            ### Description
            Per the thesis's "UI testing across multiple platforms and different screen sizes" goal, define and execute a structured test matrix (Chrome, Edge, Safari, Firefox × mobile/tablet/desktop) covering both `Heiwase.App.Blazor` and `Heiwase.App.BlazorAdmin`. Record and triage defects, filing follow-up bug issues.
            
            ### Acceptance Criteria (AC)
            Given the defined browser/device test matrix,
            When each combination is exercised (manually or via a tool such as Playwright) against both apps,
            Then results (pass/fail per combination) are documented in a single test report.
            
            Given a defect is found,
            When triaged,
            Then a new bug issue is filed referencing the specific browser/device/breakpoint.
            
            ### Definition of Done (DoD)
            - [ ] Browser/device test matrix defined and documented.
            - [ ] Public site and admin CMS exercised against every matrix entry.
            - [ ] All discovered defects filed as individual bug issues and linked from the test report.
            ```

   4. **[#126 — Finalize User & Developer Documentation](https://github.com/zzsoltg/HeiwaseWeb/issues/126)** — `OPEN`
      - **Labels:** documentation
      - **Full description (verbatim issue body):**
         ```markdown
         ### User Story
         As a future maintainer or a club administrator, I want complete, accurate documentation so that I can operate, extend, or hand over the platform without relying on the original developer's memory.
         
         ### Description
         Per the thesis's March goal of "Finalizing user and developer documentation," replace `docs/PLACEHOLDER.md` with real content covering both technical architecture (for developers) and day-to-day CMS usage (for administrators). This also retroactively captures the thesis's September technology-comparison and requirements-gathering tasks as an Architecture Decision Record.
         
         ### Acceptance Criteria (AC)
         Given a new developer cloning the repository,
         When they open `docs/`,
         Then they find an architecture overview, a data model diagram, and setup instructions sufficient to run all 5 projects locally.
         
         Given a non-technical club administrator,
         When they open the CMS user guide,
         Then they can follow step-by-step instructions to create/edit/delete an article, manage Hall of Fame entries, and review applications.
         
         ### Definition of Done (DoD)
         - [ ] `docs/PLACEHOLDER.md` replaced with structured documentation.
         - [ ] Developer documentation and an admin user guide both published in `docs/`.
         - [ ] Documentation reviewed for accuracy against `master` and merged.
         ```

      1. **[#127 — Write Developer Documentation: Architecture, Data Model, ADRs & Setup Guide](https://github.com/zzsoltg/HeiwaseWeb/issues/127)** — `OPEN`
         - **Labels:** documentation
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a future developer (or the thesis reviewer), I want documented architecture diagrams and setup instructions so that I can understand and run the 5-module solution quickly.
            
            ### Description
            Author `docs/architecture.md` describing the 5-module Clean Architecture, including a component/dependency diagram and a Cosmos DB container/entity diagram. Include an ADR capturing the September technology comparison (Blazor Server vs. WASM vs. React+Node+Postgres vs. the chosen custom serverless architecture) and the reasoning for the final choice. Add a local setup guide covering all 5 projects and required Azure resources.
            
            ### Acceptance Criteria (AC)
            Given `docs/architecture.md`,
            When reviewed,
            Then it includes an up-to-date module dependency diagram matching the actual `.slnx` references (#73).
            
            Given the local setup guide,
            When a developer follows it on a clean machine,
            Then they can successfully run `Heiwase.App.Blazor`, `Heiwase.App.BlazorAdmin`, and `Heiwase.App.Api` locally.
            
            ### Definition of Done (DoD)
            - [ ] `docs/architecture.md` with diagrams published.
            - [ ] An ADR documenting the technology comparison and final stack decision included.
            - [ ] `docs/setup.md` walks through local environment setup for all 5 projects.
            ```

      2. **[#128 — Write Club Administrator User Guide for the CMS](https://github.com/zzsoltg/HeiwaseWeb/issues/128)** — `OPEN`
         - **Labels:** documentation
         - **Full description (verbatim issue body):**
            ```markdown
            ### User Story
            As a non-technical club leader or coach, I want a plain-language guide to the CMS so that I can publish news, manage Hall of Fame entries, and review applications without developer assistance.
            
            ### Description
            Author `docs/admin-guide.md` (bilingual `hu`/`en`, consistent with the site's existing localization) covering: logging in via Microsoft Entra ID; creating/editing/publishing a News, Event, or Result article with the Markdown editor (including image/video/quote insertion); managing Hall of Fame entries; and reviewing/marking applications as reviewed.
            
            ### Acceptance Criteria (AC)
            Given a club leader with no technical background,
            When they follow the "Publish a news article" section,
            Then they can successfully create, format, and publish an article using only the documented steps.
            
            Given the guide,
            When reviewed by the Product Owner,
            Then it accurately reflects the actual CMS UI and workflow at thesis submission time.
            
            ### Definition of Done (DoD)
            - [ ] `docs/admin-guide.md` covers login, article CRUD (News/Event/Result), Hall of Fame management, and application review.
            - [ ] Screenshots or annotated steps included for each major workflow.
            - [ ] Guide reviewed and approved by the club's administrative stakeholders (or the Product Owner).
            ```

## Totals

- Milestones: 6
- Issues and sub-issues: 75 (47 open, 28 closed)
