#region using
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
#endregion using

namespace MtHoodMiata.Web.Models
{
    public sealed class MtHoodMiataRepository : IDisposable, IMtHoodMiataRepository
    {
        #region Fields
        private bool _disposed = false;
        private MtHoodMiataEntities _context = new MtHoodMiataEntities();
        #endregion Fields

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #region ClassifiedAdType
        public ClassifiedAdType GetClassifiedAdTypeById( int id, params string[] children )
        {
            ObjectQuery<ClassifiedAdType> query =
                CreateQuery<ClassifiedAdType>( _context.ClassifiedAdTypes, children );

            return query.Where( cat => cat.ClassifiedAdTypeCode == id ).FirstOrDefault();
        }

        public IEnumerable<ClassifiedAdType> GetClassifiedAdTypes( params string[] children )
        {
            ObjectQuery<ClassifiedAdType> query =
                CreateQuery<ClassifiedAdType>( _context.ClassifiedAdTypes, children );

            return query.ToList();
        }
        #endregion ClassifiedAdType

        #region ClassifiedAd
        public ClassifiedAd GetClassifiedAdById( int id, params string[] children )
        {
            ObjectQuery<ClassifiedAd> query =
                CreateQuery<ClassifiedAd>( _context.ClassifiedAds, children );

            return query.Where( ca => ca.ClassifiedAdId == id ).FirstOrDefault();
        }

        public IEnumerable<ClassifiedAd> QueryClassifiedAds(
            Expression<Func<ClassifiedAd, bool>> condition, params string[] children )
        {
            ObjectQuery<ClassifiedAd> query =
                CreateQuery<ClassifiedAd>( _context.ClassifiedAds, children );

            return query.Where( condition ).ToList();
        }

        public void Add( ClassifiedAd item )
        {
            _context.ClassifiedAds.AddObject( item );
        }

        public void Remove( ClassifiedAd item )
        {
            _context.ClassifiedAds.DeleteObject( item );
        }
        #endregion ClassifiedAd

        #region Car
        public CarDetail GetCarById( int id, params string[] children )
        {
            ObjectQuery<CarDetail> query =
                CreateQuery<CarDetail>( _context.CarDetails, children );

            return query.Where( cd => cd.CarDetailId == id ).FirstOrDefault();
        }

        public IEnumerable<CarDetail> QueryCars(
            Expression<Func<CarDetail, bool>> condition, params string[] children )
        {
            ObjectQuery<CarDetail> query =
                CreateQuery<CarDetail>( _context.CarDetails, children );

            return query.Where( condition ).ToList(); ;
        }

        public void Add( CarDetail item )
        {
            _context.CarDetails.AddObject( item );
        }

        public void Remove( CarDetail item )
        {
            _context.CarDetails.DeleteObject( item );
        }
        #endregion Car

        #region EventInfo
        public void DeleteEventInfo( int id )
        {
            EventInfo eventInfo = GetEventInfoById( id );
            Remove( eventInfo );
            _context.SaveChanges();
        }

        public EventInfo GetEventInfoById( int id, params string[] children )
        {
            ObjectQuery<EventInfo> query =
                CreateQuery<EventInfo>( _context.EventInfoes, children );

            return query.Where( e => e.EventId == id ).FirstOrDefault();
        }

        public IEnumerable<EventInfo> GetEventInfos( params string[] children )
        {
            ObjectQuery<EventInfo> query =
                CreateQuery<EventInfo>( _context.EventInfoes, children );

            return query.ToList();
        }

        public IEnumerable<EventInfo> QueryEventInfos(
            Expression<Func<EventInfo, bool>> condition, params string[] children )
        {
            ObjectQuery<EventInfo> query =
                CreateQuery<EventInfo>( _context.EventInfoes, children );

            return query.Where( condition ).ToList();
        }

        public void Add( EventInfo item )
        {
            _context.EventInfoes.AddObject( item );
        }

        public void Remove( EventInfo item )
        {
            _context.EventInfoes.DeleteObject( item );
        }
        #endregion EventInfo

        #region MeetingInfo
        public MeetingInfo GetMeetingInfoById( int id, params string[] children )
        {
            ObjectQuery<MeetingInfo> query =
                CreateQuery<MeetingInfo>( _context.MeetingInfoes, children );

            return query.Where( m => m.MeetingId == id ).FirstOrDefault();
        }

        public IEnumerable<MeetingInfo> GetMeetingInfos( params string[] children )
        {
            ObjectQuery<MeetingInfo> query =
                CreateQuery<MeetingInfo>( _context.MeetingInfoes, children );

            return query.ToList();
        }

        public IEnumerable<MeetingInfo> QueryMeetingInfos( Expression<Func<MeetingInfo, bool>> condition, params string[] children )
        {
            ObjectQuery<MeetingInfo> query =
                CreateQuery<MeetingInfo>( _context.MeetingInfoes, children );

            return query.Where( condition ).ToList();
        }

        public void Add( MeetingInfo item )
        {
            _context.MeetingInfoes.AddObject( item );
        }

        public void Remove( MeetingInfo item )
        {
            _context.MeetingInfoes.DeleteObject( item );
        }

        public MeetingInfo GetNextMeeting()
        {
            DateTime today = DateTime.Today;

            var meetingInfo = (from m in _context.MeetingInfoes
                               where m.MeetingDate >= today
                               orderby m.MeetingDate
                               select m).FirstOrDefault();

            return meetingInfo;
        }
        #endregion MeetingInfo

        #region Membership
        public Membership GetMembershipById( int id, params string[] children )
        {
            ObjectQuery<Membership> query =
                CreateQuery<Membership>( _context.Memberships, children );

            return query.Where( m => m.MembershipId == id ).FirstOrDefault();
        }

        public IEnumerable<Membership> GetMemberships( params string[] children )
        {
            ObjectQuery<Membership> query =
                CreateQuery<Membership>( _context.Memberships, children );

            return query.ToList();
        }

        public IEnumerable<Membership> QueryMemberships(
            Expression<Func<Membership, bool>> condition, params string[] children )
        {
            ObjectQuery<Membership> query =
                CreateQuery<Membership>( _context.Memberships, children );

            return query.Where( condition ).ToList(); ;
        }

        public void Add( Membership item )
        {
            _context.Memberships.AddObject( item );
        }

        public void Remove( Membership item )
        {
            _context.Memberships.DeleteObject( item );
        }
        #endregion Membership

        #region ActiveMembership
        public IEnumerable<Membership> GetActiveMemberships( params string[] children )
        {
            DateTime cutOffDate = GetCutOffDate();
            return QueryMemberships( m => m.MembershipDueDate > cutOffDate, children );
        }

        public IEnumerable<Membership> QueryActiveMemberships(
            Expression<Func<Membership, bool>> condition, params string[] children )
        {
            IEnumerable<Membership> memberships = QueryMemberships( condition, children );

            DateTime cutOffDate = GetCutOffDate();
            return memberships.Where( m => m.MembershipDueDate > cutOffDate );
        }
        #endregion ActiveMembership

        public IEnumerable<int> GetEventYears()
        {
            var eventYears = (from e in _context.EventInfoes
                              select e.StartDate.Year).Distinct().ToList();

            return eventYears;
        }

        public IEnumerable<EventInfo> GetUpcomingEvents( int count )
        {
            DateTime today = DateTime.Today;

            var events = (from e in _context.EventInfoes
                          where e.StartDate >= today
                          orderby e.StartDate
                          select e).Take( count ).ToList();

            return events;
        }

        public IEnumerable<UpdatedItem> GetWhatsNew( int withinDayCount )
        {
            DateTime today = DateTime.Today;
            DateTime limitDate = today.AddDays( -withinDayCount );

            var ads = (from ca in _context.ClassifiedAds
                       where ca.AdPlacedDttm >= limitDate
                       select new UpdatedItem
                       {
                           ItemType = "A",
                           ItemId = 0,
                           ItemText = "Classified Ads",
                           UpdateDttm = ca.AdPlacedDttm
                       }).Take( 1 );

            var events = (from e in _context.EventInfoes
                          where e.UpdatedDttm >= limitDate && e.EndDate > today
                          select new UpdatedItem
                          {
                              ItemType = "E",
                              ItemId = e.EventId,
                              ItemText = e.EventName,
                              UpdateDttm = e.UpdatedDttm
                          });

            var meetings = (from m in _context.MeetingInfoes
                            where m.UpdatedDttm >= limitDate && m.MeetingDate > today
                            select new UpdatedItem
                            {
                                ItemType = "M",
                                ItemId = 0,
                                ItemText = m.Location,
                                UpdateDttm = m.UpdatedDttm
                            });

            return ads.Union( events ).Union( meetings ).ToList();
        }

        public IEnumerable<CarDetail> GetActiveMembershipCars()
        {
            DateTime cutOffDate = GetCutOffDate();
            return QueryCars( cd => cd.Membership.MembershipDueDate > cutOffDate, "Color.ColorFamily" );
        }

        public void UpdateMemberPassword( int membershipId, int memberNumber, string password )
        {
            var membership = GetMembershipById( membershipId );

            if( membership != null )
            {
                switch( memberNumber )
                {
                    case 1:
                        membership.Member1Password = password;
                        break;

                    case 2:
                        membership.Member2Password = password;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(
                            "memberNumber",
                            "Must be 1 or 2. Value was " + memberNumber.ToString() );
                }

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "membershipId",
                    "Membership not found for " + membershipId.ToString() );
            }
        }

        public List<Color> GetColors()
        {
            var colors = (from c in _context.Colors
                          select c).ToList();

            return colors;
        }

        public void SaveMembership( Membership membership )
        {
            var existing = GetMembershipById( membership.MembershipId );


            if( existing != null )
            {
                _context.ApplyCurrentValues( "Memberships", membership );
            }
            else
            {
                _context.AddToMemberships( membership );
            }

            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #region Private Methods
        private void Dispose( bool disposing )
        {
            // Protect from being called multiple times
            if( _disposed ) return;

            if( disposing )
            {
                // Clean up all managed resources
                if( _context != null )
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        private ObjectQuery<T> CreateQuery<T>( ObjectQuery<T> baseQuery, string[] children )
        {
            if( children != null )
            {
                foreach( string child in children )
                {
                    baseQuery = baseQuery.Include( child );
                }
            }

            return baseQuery;
        }

        private DateTime GetCutOffDate()
        {
            return DateTime.Today.AddDays( -30 );
        }
        #endregion Private Methods
    }

    public class UpdatedItem
    {
        public string ItemType { get; set; }
        public int ItemId { get; set; }
        public string ItemText { get; set; }
        public DateTime UpdateDttm { get; set; }
    }
}