# Thesis Proposal

## Development of a Cost-efficient Serverless Web Platform and Content Management System for Sports Clubs Using .NET Blazor and Azure Technologies

---

### **Candidate Information**
* **Candidate Name:** Zsolt Gábor Zimmermann  
* **Neptun Code:** IQMBGI  
* **Major:** Computer Science BSc (*Programtervező informatikus BSc*)  
* **Study Mode:** Full-time (*Nappali*)  
* **Type of Thesis:** Bachelor's Thesis (*Szakdolgozat*)  
* **Supervisor:** Dr. Zoltán Richárd Jánki  

---

## 1. Subject of the Thesis

Establishing a professional digital footprint has perpetually posed a formidable challenge for small and medium-sized enterprises (SMEs), particularly when bespoke software development is necessitated. Beyond the realm of prefabricated, template-driven website builders, the sole viable alternative often entails commissioning a specialist and bearing the burden of continuous operational expenditures. Consequently, associations or companies with constrained financial resources typically maintain static and obsolescent websites. Modifying their content is a cumbersome endeavor, while the upkeep of traditional, dedicated server infrastructures remains disproportionately exorbitant.

The focal point of my thesis is the conceptualization and development of a web platform and Content Management System (CMS) architected upon a cloud-native, serverless paradigm. This solution is meticulously tailored to circumvent the aforementioned predicaments for my sports team, ensuring the revitalization of their online presence. Furthermore, it empowers team members, coaches, and club executives—even those devoid of software engineering acumen—to seamlessly manage and edit published content.

The project will encompass two distinct client-side Single Page Applications (SPAs). 
1. **The Public Portal:** The first is the public-facing website of the association, constructed in alignment with contemporary marketing trends and landing page layout principles, featuring a bespoke visual identity and design elements. Beyond its introductory role, the primary objective of this portal is to dynamically showcase the latest news, relevant information, and athletic achievements pertaining to the team, whilst streamlining the application process for prospective members.
2. **The Administrative Interface:** The second application is a sophisticated administrative dashboard, granting the association's leadership the capability to execute comprehensive CRUD (*Create, Read, Update, Delete*) operations on the news and results displayed on the public portal. Article formatting is facilitated by an integrated Markdown editor, which seamlessly accommodates the embedding of images, videos, and blockquotes. The public interface subsequently renders this Markdown-formatted data, meticulously applying the club's corporate styling guidelines.

The provisioning of client applications and the orchestration of database interactions will be governed by a robust, Azure Functions-based serverless REST API layer. This API securely intercepts client requests via authenticated channels, executes the requisite business logic, and interfaces with a cloud-based NoSQL Azure Cosmos database designated for news storage, alongside an Azure Blob Storage repository serving multimedia assets. This architectural topology guarantees uncompromised cost-efficiency and presents a cutting-edge alternative to conventional, server-centric backend infrastructures. 

The entire technological stack is exclusively predicated on the ecosystem provided by Microsoft, intrinsically leveraging the .NET framework and Azure cloud services.

The ultimate objective of my thesis is to deliver a robust, high-fidelity software product that is instantaneously viable for deployment in a live production environment, seamlessly integrating into and actively elevating the daily operations of the sports association.

---

## 2. Applied Technologies

* **Web Applications:** Blazor WebAssembly Standalone (.NET, C#, HTML, Less, CSS, JavaScript)
* **Backend & Database:** Azure Functions, Azure Cosmos DB, Azure Blob Storage (.NET, C#, Markdown)

---

## 3. Detailed Schedule

### **September 2026 – Planning and Frontend Development**
* Gathering the functional and non-functional requirements of the sports association.
* Evaluating and benchmarking client-side, server-side, and database technologies (*Blazor Server vs. traditional Blazor WebAssembly vs. React + Node.js + Postgres vs. Custom serverless architecture*).
* Developing the static Blazor WebAssembly Standalone application.
* Crafting a responsive UI and a bespoke design system. Creating reusable components by leveraging Less files and mixins.
* Mitigating the 250 MB Azure Static Web App limitation, and preparing the infrastructure for manual Blob Storage deployments.
* CI/CD foundations: Configuring GitHub Actions for the automated static publishing of the public-facing site.

### **October 2026 – Multi-module Architecture and Azure Cloud Services**
* Designing the high-level system architecture and establishing robust module dependencies.
* Decomposing the project into five distinct modules: `Blazor`, `BlazorAdmin`, `Shared`, `Api`, and `Infrastructure`.
* Designing and provisioning the Azure cloud infrastructure: Cosmos DB, Blob Storage, and Azure Functions.
* Defining data models, entities, and Data Transfer Objects (DTOs) within the `Shared` project.
* Establishing database connectivity paradigms within the `Infrastructure` layer.

### **December 2026 – Backend and Azure Functions Integration**
* Developing serverless REST API endpoints utilizing an Azure Functions v4 project architecture.
* Implementing Microsoft Entra ID-based authentication and authorization protocols across all protected endpoints.
* Transitioning the public landing page from utilizing static JSON payloads to dynamically querying real-time data from Cosmos DB.
* Implementing HTTP client services within the Blazor application to facilitate seamless API communication.
* Integrating Azure Application Insights to establish comprehensive telemetry, logging, and exception handling mechanisms.

### **January 2027 – Administrative Interface Development**
* Comprehensive realization of the highly secure `BlazorAdmin` CMS interface.
* Implementation of ubiquitous CRUD operations for managing news articles and events.
* Integration of a sophisticated, embedded Markdown editor.
* Development of a dedicated file upload service interfacing directly with Azure Blob Storage.
* Incorporating Progressive Web Application (PWA) functionalities, Service Workers, and offline caching mechanisms.

### **March 2027 – DevOps Processes and Rigorous Testing**
* Authoring comprehensive Unit tests to ensure code reliability.
* Integrating automated testing protocols seamlessly into the CI/CD pipeline.
* Finalizing the dual CI/CD pipelines within the GitHub Actions environment.
* Conducting rigorous UI testing across diverse platforms and varying screen resolutions.
* Finalizing both end-user and developer documentation.

### **April 2027 – Thesis Initiation: Design, Implementation, and Testing Chapters**
* Generating architectural schematics and entity-relationship database diagrams.
* Compiling a meticulously detailed table of contents and drafting the thesis introduction.
* Exhaustively documenting the system's architectural blueprint.
* Explicating the serverless REST API workflows and outlining the underlying data model.
* Providing an in-depth operational exposition of the Content Management System.

### **May 2027 – Testing Chapter, Final Manuscript, and Submission**
* Showcasing the CI/CD automation capabilities and the GitHub Actions pipeline.
* Presenting the Unit tests alongside a profound, code-level analytical review.
* Evaluating Application Insights error logs and performance metrics, supplemented with visual diagrams.
* Cataloging the system's inherent limitations and outlining prospective avenues for future enhancements.
* Final synthesis, comprehensive formatting, referencing, and rigorous plagiarism verification.
* Final submission of the thesis and intensive preparation for the academic defense.

---

## 4. Planned Features

### **Core Features**
* A public, contemporary introductory interface adhering to modern landing page conventions.
* Dynamic rendering of the latest news, upcoming events, and athletic results.
* An intuitive application form for prospective members, meticulously capturing and recording incoming inquiries.
* An isolated, highly secure administrative dashboard exclusively dedicated to content management.
* Comprehensive CRUD capabilities applied to published articles and content.
* An embedded Markdown editor boasting innate support for images, videos, and brand-specific structural elements (e.g., blockquotes, custom headers).
* A serverless, Azure Functions-driven REST API orchestrating seamless data interchange between client applications and the database.
* Cost-optimized, highly scalable data persistence utilizing a cloud-native NoSQL database and Blob storage.
* A fully responsive user interface seamlessly adapting to both desktop and mobile viewports.

### **Supplementary Features**
* Systematic listing and comprehensive state management of received membership applications within the administrative portal.
* Dynamic Search Engine Optimization (SEO) and robust metadata management on the public-facing site.
* Automated email notifications instantaneously dispatched to the management board upon the receipt of new membership applications.
* Progressive Web Application (PWA) support to ensure accelerated loading times and partial offline accessibility for the administrative client.
* Continuous Integration and Continuous Deployment (CI/CD) automation elegantly orchestrated via GitHub Actions.