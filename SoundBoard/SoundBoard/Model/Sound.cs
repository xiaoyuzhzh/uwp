using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundBoard.Model
{
    public class Sound
    {
        public String Name { get; set; }
        public SoundCategory Category { get; set; }
        public String AudioFile { get; set; }
        public String ImageFile { get; set; }

        public Sound(String name,SoundCategory category)
        {
            Name = name;
            Category = category;
            AudioFile = string.Format("/Assets/Audio/{0}/{1}.wav", category, name);
            ImageFile = string.Format("/Assets/Images/{0}/{1}.png", category, name);
        }
    }


    public enum SoundCategory
    {
        Animals,
        Cartoons,
        Taunts,
        Warnings,
        Test
    }
}
