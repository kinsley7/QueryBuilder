using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**       
 *--------------------------------------------------------------------
 * 	   File name: BannedGames
 * 	Project name:CrowdisLab3
 *--------------------------------------------------------------------
 * Author’s name and email:	 kinsley crowdis crowdis@etsu.edu			
 *          Course-Section: CSCI 2910-800
 *           Creation Date:	09/14/2023
 * -------------------------------------------------------------------
 */

namespace CrowdisLab3
{
    internal class BannedGame : IClassModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Series { get; set; }
        public string Country { get; set; }
        public string? Details { get; set; }

        public BannedGame() { }
        public BannedGame(int id, string title, string series, string country, string? details)
        {
            Id = id;
            Title = title;
            Series = series;
            Country = country;
            Details = details;
        }

        public override string ToString()
        {
            string msg = "";

            msg += $"Id: {Id}\n";
            msg += $"Title: {Title}\n";
            msg += $"Series: {Series} \n";
            msg += $"Country: {Country} \n";
            msg += $"Details: {Details} \n";

            return msg;
        }
    }
}
