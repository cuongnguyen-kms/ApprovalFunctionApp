﻿# ApprovalFunctionApp

## 📌 Overview
ApprovalFunctionApp is an **Azure Function App** built using **C# (.NET Isolated)** and **Durable Functions** to handle an approval workflow. It allows users to **start an approval process, approve, or reject** a request while sending email notifications.

---

## 🚀 Features
✅ **Durable Orchestration** - Handles the approval workflow asynchronously.  
✅ **Event-Driven Approval Handling** - Listens for approval events and processes accordingly.  
✅ **Email Notifications** - Sends email updates for each approval step.  
✅ **Structured DTOs** - Separates function layer and service layer with DTOs.  
✅ **Configurable Settings** - Uses `EmailSettings` for SMTP configuration.  

---

## 🏗️ Project Structure
```
ApprovalFunctionApp/
│── Configurations/
│   ├── EmailSettings.cs         # Email configuration settings
│── Constants/
│   ├── AppConstants.cs          # Constant values used across the application
│   ├── FunctionRoutes.cs        # API route constants
│── Dtos/
│   ├── ApprovalRequestDto.cs    # DTO for incoming approval requests
│   ├── ApprovalEventDto.cs      # DTO for approval actions (approve/reject/cancel)
│── Interfaces/
│   ├── IApprovalService.cs      # Interface for Approval Service
│   ├── IEmailService.cs         # Interface for Email Service
│── Models/
│   ├── ApprovalRequest.cs       # Domain model for approval requests
│   ├── EmailData.cs             # Model for email notifications
│── Services/
│   ├── ApprovalService.cs       # Business logic for managing approval workflows
│── ApprovalWorkflow.cs          # Azure Functions entry point
│── Program.cs                   # Function app startup configuration
```

---

## ⚙️ Setup & Requirements
### 🔹 Prerequisites
- **.NET SDK 8+** [(Download)](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Azure Functions Core Tools** (Install via npm):
  ```sh
  npm install -g azure-functions-core-tools@4 --unsafe-perm true
  ```
- **Azure CLI** [(Install)](https://aka.ms/installazurecliwindows)
- **An Azure Subscription** (Sign up at [Azure Portal](https://portal.azure.com))
- **Postman or cURL** (for API testing)

### 🔹 Clone the Repository
```sh
git clone https://github.com/YOUR_GITHUB/ApprovalFunctionApp.git
cd ApprovalFunctionApp
```

### 🔹 Install Dependencies
```sh
dotnet restore
```

---

## 🏃 Running Locally
### 🔹 Configure Local Settings
Update `local.settings.json` with your Azure Storage and Email settings:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "EmailSettings:Provider": "<your_email_provider>",
    "EmailSettings:Username": "<your_email_user_name>",
    "EmailSettings:Password": "<your_password>",
    "EmailSettings:SenderEmail": "<sender_email>",
    "EmailSettings:SmtpServer": "<smtp_server>",
    "EmailSettings:SmtpPort": "<smtp_port>"
  }
}
```

### 🔹 Start the Function App Locally
```sh
func start
```

### 🔹 Test with Postman or cURL
#### 📌 Start Approval
```sh
curl -X POST "http://localhost:7196/api/approval/start" \
     -H "Content-Type: application/json" \
     -d '{"RequesterEmail": "test@example.com"}'
```
#### 📌 Approve Request
```sh
curl -X POST "http://localhost:7196/api/approval/approve" \
     -H "Content-Type: application/json" \
     -d '{"InstanceId": "YOUR_INSTANCE_ID"}'
```
#### 📌 Reject Request
```sh
curl -X POST "http://localhost:7196/api/approval/reject" \
     -H "Content-Type: application/json" \
     -d '{"InstanceId": "YOUR_INSTANCE_ID"}'
```

---

## 🌍 Deploying to Azure
### 🔹 Log in to Azure
```sh
az login
```

### 🔹 Create Azure Resources
```sh
az functionapp create --resource-group YOUR_RESOURCE_GROUP \
    --consumption-plan-location YOUR_REGION \
    --runtime dotnet-isolated \
    --runtime-version 8 \
    --functions-version 4 \
    --name YOUR_FUNCTION_APP_NAME \
    --storage-account YOUR_STORAGE_ACCOUNT \
    --os-type Windows
```

### 🔹 Deploy the Function
```sh
func azure functionapp publish YOUR_FUNCTION_APP_NAME
```

---

## 📜 License
This project is licensed under the **MIT License**.

---

## 📞 Need Help?
- **Issues:** Submit an issue on GitHub.
- **Documentation:** Read more about Durable Functions [here](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview).
- **Azure Support:** [Microsoft Azure Support](https://azure.microsoft.com/en-us/support/).

🚀 Happy Coding! 🎯

