using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Settings;
public record FileSettings()
{
  public string MoviePicturesPath { get; init; }
  public string ActorPicturesPath { get; init; }
  public string DirectorPicturesPath { get; init; }
  public string ResourcesPath { get; init; }
  public string PlaceholderPicturePath { get; init; }
  public string MoviePostersPath { get; init; }
}