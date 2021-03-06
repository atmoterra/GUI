#region Copyright
///<remarks>
/// <GRAL Graphical User Interface GUI>
/// Copyright (C) [2019]  [Dietmar Oettl, Markus Kuntner]
/// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by
/// the Free Software Foundation version 3 of the License
/// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
/// You should have received a copy of the GNU General Public License along with this program.  If not, see <https://www.gnu.org/licenses/>.
///</remarks>
#endregion

/*
 * Created by SharpDevelop.
 * User: U0178969
 * Date: 24.01.2019
 * Time: 14:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using GralItemData;
using System.Drawing;

namespace GralDomain
{
    public partial class Domain
    {
        /// <summary>
        /// Start the vegetation dialog (checkbox26 = checked) or save the vegetation data (checkbox26 = unchecked)
        /// </summary>
        /// <param name="sender">if checkbox26.checked == false and sender == null -> EditVegetation.SaveArray not called</param>
        void EditAndSaveVegetationData(object sender, EventArgs e)
        {
            VegetationToolStripMenuItem.Checked = checkBox26.Checked;
            
            if (checkBox26.Checked == true)
            {
                HideWindows(26); // Kuntner
                
                if (ShowFirst.Veg) // set the inital position of the form
                {
                    if (ShowFirst.Ls == false)
                    {
                        EditVegetation.Location = EditLS.Location;
                    }
                    else if (ShowFirst.As == false)
                    {
                        EditVegetation.Location = EditAS.Location;
                    }
                    else if (ShowFirst.Ps == false)
                    {
                        EditVegetation.Location = EditPS.Location;
                    }
                    else if (ShowFirst.Wa == false)
                    {
                        EditVegetation.Location = EditWall.Location;
                    }
                    else if (ShowFirst.Bu == false)
                    {
                        EditVegetation.Location = EditB.Location;
                    }
                    else
                    {
                        EditVegetation.Location = GetScreenPositionForNewDialog();
                    }

                    ShowFirst.Veg = false;
                }
                MouseControl = 79;
                InfoBoxCloseAllForms(); // close all infoboxes
                EditVegetation.Show();
                EditVegetation.TopMost = true; // Kuntner
                EditVegetation.ShowForm();
                Cursor = Cursors.Cross;
                
                CheckForExistingDrawingObject("VEGETATION");  
            }
            else
            {
                EditVegetation.Hide(); // Kuntner first hide form to save actual sourcedata
                if (Gral.Main.Project_Locked == true)
                {
                    //Gral.Main.Project_Locked_Message(); // Project locked!
                    MouseControl = 0;
                    Picturebox1_Paint();
                }
                else if (MainForm.DeleteGralGffFile() == DialogResult.OK) // Warningmessage if gff Files exist!
                {
                    if (sender != null) // do not use the dialogue data, if data has been changed outisde the EditPortals dialogue
                    {
                        EditVegetation.SaveArray();
                    }
                    //save Vegetation input to file
                    VegetationDataIO _veg = new VegetationDataIO();
                    _veg.SaveVegetation(EditVegetation.ItemData, Gral.Main.ProjectName, Gral.Main.CompatibilityToVersion1901);
                    _veg = null;
                    
                    Cursor = Cursors.Default;
                    MouseControl = 0;
                    //this.Width = ScreenWidth;
                    //this.Height = ScreenHeight - 50;
                    for (int i = 0; i <= EditVegetation.CornerVegetation; i++)
                    {
                        CornerAreaSource[EditVegetation.CornerVegetation] = new Point();
                    }
                    
                    EditVegetation.CornerVegetation = 0;
                    Picturebox1_Paint();
                    MainForm.Change_Label(3, 0); // Building label red & delete buildings.dat
                    
                    if (MainForm.GRALSettings.BuildingMode != 0)
                    {
                        if (EditVegetation.ItemData.Count > 0)
                        {
                            MainForm.Change_Label(3, 0); // Building label red & delete buildings.dat
                            MainForm.button9.Visible = true;
                        }
                        else
                        {
                            MainForm.Change_Label(3, -1); // Building label - no buildings
                        }
                    }
                    
                    //add/delete vegetation in object list
                    if (EditVegetation.ItemData.Count == 0)
                    {
                        RemoveItemFromItemOptions("VEGETATION");
                    }
                }
            }
            
            //show/hide button to select buildings
            if (EditVegetation.ItemData.Count > 0)
            {
                button50.Visible = true;
            }
            else
            {
                button50.Visible = false;
            }

            //enable/disable GRAL simulations
            MainForm.Enable_GRAL();
            //enable/disable GRAMM simulations
            MainForm.Enable_GRAMM();
        }
    }
}