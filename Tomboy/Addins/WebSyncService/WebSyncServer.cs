// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Sandy Armstrong <sanfordarmstrong@gmail.com>
// 

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Tomboy.Sync;
using Tomboy.WebSync.Api;

namespace Tomboy.WebSync
{
	public class WebSyncServer : SyncServer
	{
		private string rootUri;
		private IWebConnection connection;

		private RootInfo root;
		private UserInfo user;
		private List<NoteInfo> pendingCommits;
		
		public WebSyncServer (string serverUrl, IWebConnection connection)
		{
			Logger.Debug ("Constructing WebSyncServer at {0} with {1}", serverUrl, connection);
			this.connection = connection;
			rootUri = serverUrl.TrimEnd ('/') + "/api/1.0/";
		}

		#region SyncServer implementation
		
		public bool BeginSyncTransaction ()
		{
			Logger.Debug ("BeginSyncTransaction ()");
			// TODO: Check connection and auth (is getting root/user resources a sufficient check?)
			root = RootInfo.GetRoot (rootUri, connection);
			user = UserInfo.GetUser (root.User.ApiRef, connection);
			if (user.LatestSyncRevision.HasValue)
				LatestRevision = user.LatestSyncRevision.Value;
			else
				VerifyLatestSyncRevision (user.LatestSyncRevision);

			if (string.IsNullOrEmpty (user.CurrentSyncGuid))
				throw new TomboySyncException ("No sync GUID for user provided in server response");
			
			pendingCommits = new List<NoteInfo> ();
			return true;
		}
		
		public bool CancelSyncTransaction ()
		{
			Logger.Debug ("CancelSyncTransaction ()");
			// TODO: Cancel any pending request
			pendingCommits.Clear ();
			return true;
		}
		
		public bool CommitSyncTransaction ()
		{
			Logger.Debug ("CommitSyncTransaction ()");
			if (pendingCommits != null && pendingCommits.Count > 0) {
				LatestRevision = user.UpdateNotes (pendingCommits, LatestRevision + 1);
				pendingCommits.Clear ();
			}
			return true;
		}
		
		public SyncLockInfo CurrentSyncLock {
			get {
				Logger.Debug ("CurrentSyncLock");
				return null;
			}
		}
		
		public void DeleteNotes (IList<string> deletedNoteUUIDs)
		{
			Logger.Debug ("Called DeleteNotes {0}", deletedNoteUUIDs);
			foreach (string uuid in deletedNoteUUIDs) {
				NoteInfo noteInfo = new NoteInfo ();
				noteInfo.Command = "delete";
				noteInfo.Guid = uuid;
				pendingCommits.Add (noteInfo);
			}
		}
		
		public IList<string> GetAllNoteUUIDs ()
		{
			Logger.Debug ("GetAllNoteUUIDs called");
			List<string> uuids = new List<string> ();
			int? latestRevision;
			IList<NoteInfo> serverNotes = user.GetNotes (false, out latestRevision);
			VerifyLatestSyncRevision (latestRevision);
			foreach (NoteInfo noteInfo in serverNotes)
				uuids.Add (noteInfo.Guid);
			return uuids;
		}
		
		public IDictionary<string, NoteUpdate> GetNoteUpdatesSince (int revision)
		{
			Logger.Debug ("GetNoteUpdatesSince {0} called", revision);
			Dictionary<string, NoteUpdate> updates =
				new Dictionary<string, NoteUpdate> ();
			int? latestRevision;
			IList<NoteInfo> serverNotes = user.GetNotes (true, revision, out latestRevision);
			VerifyLatestSyncRevision (latestRevision);
			foreach (NoteInfo noteInfo in serverNotes) {
				string noteXml = NoteConvert.ToNoteXml (noteInfo);
				NoteUpdate update = new NoteUpdate (noteXml,
				                                    noteInfo.Title,
				                                    noteInfo.Guid,
				                                    noteInfo.LastSyncRevision.Value);
				updates.Add (noteInfo.Guid, update);
			}
			return updates;
		}
		
		public string Id {
			get {
				Logger.Debug ("Sync UserID {0}", user.CurrentSyncGuid);
				return user.CurrentSyncGuid;
			}
		}
		
		public int LatestRevision { get; private set; }
		
		public void UploadNotes (IList<Note> notes)
		{
			Logger.Debug ("UploadNotes {0} called", notes);
			foreach (Note note in notes)
				pendingCommits.Add (NoteConvert.ToNoteInfo (note));
		}

		public bool UpdatesAvailableSince (int revision)
		{
			Logger.Debug ("UpdatesAvailableSince {0}", revision);
			root = RootInfo.GetRoot (rootUri, connection);
			user = UserInfo.GetUser (root.User.ApiRef, connection);
			return user.LatestSyncRevision.HasValue &&
				user.LatestSyncRevision.Value > revision;
		}
		
		#endregion

		#region Private Methods

		private void VerifyLatestSyncRevision (int? latestRevision)
		{
			Logger.Debug ("VerifyLatestSyncRevision {0} called", latestRevision);
			if (!latestRevision.HasValue)
				throw new TomboySyncException ("No sync revision provided in server response");
			if (latestRevision.Value != LatestRevision)
				throw new TomboySyncException ("Latest revision on server has changed, please update restart your sync");
		}

		#endregion
	}
}
