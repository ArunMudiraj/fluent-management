﻿/************************************************************************************************************
 * This software is distributed under a GNU Lesser License by Elastacloud Limited and it is free to         *
 * modify and distribute providing the terms of the license are followed. From the root of the source the   *
 * license can be found in /Resources/license.txt                                                           * 
 *                                                                                                          *
 * Web at: www.elastacloud.com                                                                              *
 * Email: info@elastacloud.com                                                                              *
 ************************************************************************************************************/

using System.Xml.Linq;
using Elastacloud.AzureManagement.Fluent.Commands.Services;
using Elastacloud.AzureManagement.Fluent.Helpers;
using Elastacloud.AzureManagement.Fluent.Types;
using Elastacloud.AzureManagement.Fluent.Types.VirtualMachines;
using Elastacloud.AzureManagement.Fluent.VirtualMachines.Classes;
using Deployment = Elastacloud.AzureManagement.Fluent.Types.VirtualMachines.Deployment;

namespace Elastacloud.AzureManagement.Fluent.Commands.VirtualMachines
{
    /// <summary>
    /// Creates a deployment for a virtual machine and allows some preconfigured defaults from the image gallery 
    /// </summary>
    internal class CreateWindowsVirtualMachineDeploymentCommand : ServiceCommand
    {
        // https://management.core.windows.net/<subscription-id>/services/hostedservices/<service-name>/deployments/
        /// <summary>
        /// Used to construct the command to create a virtual machine deployment including the creation of a role
        /// </summary>
        internal CreateWindowsVirtualMachineDeploymentCommand(WindowsVirtualMachineProperties properties)
        {
            AdditionalHeaders["x-ms-version"] = "2012-03-01";
            OperationId = "hostedservices";
            ServiceType = "services";
            HttpCommand = properties.CloudServiceName + "/deployments";
            VirtualMachineType = properties.VirtualMachineType;
            CloudServiceName = properties.CloudServiceName;
            VhdStorageAccount = properties.StorageAccountName;
            Size = properties.VmSize;
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets the virtual machine template
        /// </summary>
        public VirtualMachineTemplates VirtualMachineType { get; set; }

        /// <summary>
        /// The name of the cloud service to which the virtual machine will be created 
        /// </summary>
        public string CloudServiceName { get; set; }

        /// <summary>
        /// The storage account where the VHD will be stored
        /// </summary>
        public string VhdStorageAccount { get; set; }

        /// <summary>
        /// The size of the virtual machine
        /// </summary>
        public VmSize Size { get; set; }
        /// <summary>
        /// The full virtual machine properties of the windows instance the needs to be deployed
        /// </summary>
        public WindowsVirtualMachineProperties Properties { get; set; }

        /// <summary>
        /// Creates a deployment payload for a predefined template 
        /// </summary>
        /// <returns>A string xml representation</returns>
        protected override string CreatePayload()
        {
            var deployment = Deployment.GetAdHocWindowsTemplateDeployment(Properties);
            var document = new XDocument(deployment.GetXmlTree());
            return document.ToStringFullXmlDeclarationWithReplace();
        }

        /// <summary>
        /// returns the name of the command
        /// </summary>
        public override string ToString()
        {
            return "CreateWindowsVirtualMachineDeploymentCommand";
        }
    }
}