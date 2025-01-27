# NetProject-TestDaryaVaria
Test Project for Job Applicant

# Medicine Management System

This is a Medicine Management System built using .NET MVC with Web API, integrated with a Microsoft SQL Server database. The system allows for role-based login and menu access, customer medicine requests, approval workflows, and admin capabilities for managing the medicine stock. It also provides charts to visualize medicine stock and expiration details using Chart.js.

## Features

### 1. **Role-based Login and Menu:**
   - Users can log in with specific roles (Admin, Customer).
   - The menu dynamically adjusts based on the user's role.

### 2. **Customer Medicine Request:**
   - Customers can submit requests for medicines.
   - The system allows for tracking of medicine request statuses.

### 3. **Admin Approval for Medicine Requests:**
   - Admins can review and approve or deny customer medicine requests.
   - Admin can also monitor pending requests.

### 4. **Admin Can Add New Medicine:**
   - Admin can add new medicines to the system, including stock details, expiry dates, and other relevant information.

### 5. **Chart.js Integration:**
   - Uses Chart.js for visualizing:
     - **Medicine Stock Levels**
     - **Medicine Expiry Dates**

### 6. **Database:**
   - The system uses a Microsoft SQL Server database for storing data about users, medicine, requests, etc.

## Technologies Used
- **.NET MVC**
- **Web API**
- **Microsoft SQL Server**
- **Chart.js**
- **HTML/CSS/JavaScript**
- **Entity Framework for ORM**