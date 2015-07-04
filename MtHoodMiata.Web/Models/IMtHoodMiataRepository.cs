#region using
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#endregion using

namespace MtHoodMiata.Web.Models
{
    public interface IMtHoodMiataRepository
    {
        void SaveChanges();
        void Dispose();

        MeetingInfo GetMeetingInfoById( int id, params string[] children );
        IEnumerable<MeetingInfo> GetMeetingInfos( params string[] children );
        IEnumerable<MeetingInfo> QueryMeetingInfos( Expression<Func<MeetingInfo, bool>> condition, params string[] children );
        void Add( MeetingInfo item );
        void Remove( MeetingInfo item );
        MeetingInfo GetNextMeeting();

        ClassifiedAd GetClassifiedAdById( int id, params string[] children );
        IEnumerable<ClassifiedAd> QueryClassifiedAds( Expression<Func<ClassifiedAd, bool>> condition, params string[] children );
        void Add( ClassifiedAd item );
        void Remove( ClassifiedAd item );

        CarDetail GetCarById( int id, params string[] children );
        IEnumerable<CarDetail> QueryCars( Expression<Func<CarDetail, bool>> condition, params string[] children );
        void Add( CarDetail item );
        void Remove( CarDetail item );
        IEnumerable<CarDetail> GetActiveMembershipCars();

        EventInfo GetEventInfoById( int id, params string[] children );
        IEnumerable<EventInfo> GetEventInfos( params string[] children );
        IEnumerable<EventInfo> QueryEventInfos( Expression<Func<EventInfo, bool>> condition, params string[] children );
        void Add( EventInfo item );
        void Remove( EventInfo item );
        void DeleteEventInfo( int id );
        IEnumerable<int> GetEventYears();
        IEnumerable<EventInfo> GetUpcomingEvents( int count );

        Membership GetMembershipById( int id, params string[] children );
        IEnumerable<Membership> GetMemberships( params string[] children );
        IEnumerable<Membership> QueryMemberships( Expression<Func<Membership, bool>> condition, params string[] children );
        void Add( Membership item );
        void Remove( Membership item );
        void UpdateMemberPassword( int membershipId, int memberNumber, string password );

        IEnumerable<Membership> GetActiveMemberships( params string[] children );
        IEnumerable<Membership> QueryActiveMemberships( Expression<Func<Membership, bool>> condition, params string[] children );

        ClassifiedAdType GetClassifiedAdTypeById( int id, params string[] children );
        IEnumerable<ClassifiedAdType> GetClassifiedAdTypes( params string[] children );

        List<Color> GetColors();

        IEnumerable<UpdatedItem> GetWhatsNew( int withinDayCount );
    }
}
