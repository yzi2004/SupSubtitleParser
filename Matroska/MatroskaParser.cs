using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupSubtitleParser.Matroska
{
    //class MatroskaParser
    //{
    //    private void ImportSubtitleFromMatroskaFile(string fileName)
    //    {
    //        using (var matroska = new MatroskaFile(fileName))
    //        {
    //            if (matroska.IsValid)
    //            {
    //                var subtitleList = matroska.GetTracks(true);
    //                if (subtitleList.Count == 0)
    //                {
    //                    //No Data
    //                }
    //                else
    //                {
    //                    LoadMatroskaSubtitle(subtitleList[0], matroska, false);
    //                }
    //            }
    //        }
    //    }

    //    private bool LoadMatroskaSubtitle(MatroskaTrackInfo matroskaSubtitleInfo, MatroskaFile matroska, bool batchMode)
    //    {
    //        if (matroskaSubtitleInfo.CodecId.Equals("S_HDMV/PGS", StringComparison.OrdinalIgnoreCase))
    //        {
    //            if (batchMode)
    //                return false;
    //            return LoadBluRaySubFromMatroska(matroskaSubtitleInfo, matroska);
    //        }

            
    //        var sub = matroska.GetSubtitle(matroskaSubtitleInfo.TrackNumber, MatroskaProgress);
    //        TaskbarList.SetProgressState(Handle, TaskbarButtonProgressFlags.NoProgress);
    //        Cursor.Current = Cursors.Default;

    //        MakeHistoryForUndo(_language.BeforeImportFromMatroskaFile);
    //        _subtitleListViewIndex = -1;
    //        if (!batchMode)
    //            ResetSubtitle();
    //        _subtitle.Paragraphs.Clear();

    //        var format = Utilities.LoadMatroskaTextSubtitle(matroskaSubtitleInfo, matroska, sub, _subtitle);

    //        if (matroskaSubtitleInfo.CodecPrivate.Contains("[script info]", StringComparison.OrdinalIgnoreCase))
    //        {
    //            if (_networkSession == null)
    //            {
    //                SubtitleListview1.ShowExtraColumn(_languageGeneral.Style);
    //                SubtitleListview1.DisplayExtraFromExtra = true;
    //            }
    //        }
    //        else if (_networkSession == null && SubtitleListview1.IsExtraColumnVisible)
    //        {
    //            SubtitleListview1.HideExtraColumn();
    //        }
    //        comboBoxSubtitleFormats.SelectedIndexChanged -= ComboBoxSubtitleFormatsSelectedIndexChanged;
    //        SetCurrentFormat(format);
    //        comboBoxSubtitleFormats.SelectedIndexChanged += ComboBoxSubtitleFormatsSelectedIndexChanged;
    //        SetEncoding(Encoding.UTF8);
    //        ShowStatus(_language.SubtitleImportedFromMatroskaFile);
    //        _subtitle.Renumber();
    //        _subtitle.WasLoadedWithFrameNumbers = false;
    //        if (matroska.Path.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase) || matroska.Path.EndsWith(".mks", StringComparison.OrdinalIgnoreCase))
    //        {
    //            _fileName = matroska.Path.Remove(matroska.Path.Length - 4);
    //            Text = Title + " - " + _fileName;
    //        }
    //        else
    //        {
    //            Text = Title;
    //        }
    //        _fileDateTime = new DateTime();

    //        _converted = true;

    //        if (batchMode)
    //            return true;

    //        SubtitleListview1.Fill(_subtitle, _subtitleAlternate);
    //        if (_subtitle.Paragraphs.Count > 0)
    //            SubtitleListview1.SelectIndexAndEnsureVisible(0);

    //        ShowSource();
    //        return true;
    //    }

    //    private bool LoadBluRaySubFromMatroska(MatroskaTrackInfo matroskaSubtitleInfo, MatroskaFile matroska)
    //    {


    //        var sub = matroska.GetSubtitle(matroskaSubtitleInfo.TrackNumber, null);

    //        return true;
    //    }
    //}
}
