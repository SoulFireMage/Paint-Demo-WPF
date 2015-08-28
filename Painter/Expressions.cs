//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//THIS HAS NOTHING TO DO WITH THE PAINTER APPLICATION! IT GOT PULLED INTO THE PROJECT
//DUE TO RUSHING ABOUT TRYING TO SAVE STUFF?
//namespace Painter
//{
 
//    /// <summary>
//    ///This generic class encapsulates the main algorithms for building a rule engine and a list of matched source/merge 
//    ///"rows" To prove whether a row in source and target are equivalent. 
//    /// A property in the classes we may be using represent columns in a table or query.
//    ///Say we have an instance thus: 
//    ///SourcePerson = New Person with {.Name = "ommitted db query", 
//    ///                                .DOB =" ommitted db query"}
//    ///MergedPerson = New Person with {.Name = "ommitted db query", 
//    ///                                .DOB =" ommitted db query"}
//    ///We add these instances to a List(of Person) so we get SourceList(of Person), MergeList(Of Person).
//    ///Then we would like to build some list where each item is a match pair of Persons. The engine below can be told to match on .Name and .DOB; 
//    ///it will generate the rules as needed and use them to decide if instances from each list are in fact, the same "fact". 
//    ///
//    ///So we get a List(of MatchedPair(Of Person)) where MatchPair has members .SourcePerson and .MergePerson. In addition, 
//    ///using the IName, the MatchedPair instance will guarantee an Overrides ToString that returns a member from MergePerson (in this case).
//    /// </summary>
//    ///<typeparam name="T"></typeparam>
//    ///<remarks></remarks>

//    class BuildGenericMergeList<T> where T : INamed
//    {
//        private List<T> _SourceList { get; set; }
//        public List<T> SourceList
//        {
//            get { return _SourceList; }
//        }

//        private List<T> _MergeList { get; set; }
//        public List<T> Mergelist
//        {
//            get { return _MergeList; }
//        }

//        private List<T> _unMatchedSource { get; set; }
//        public List<T> unMatchedSource
//        {
//            get
//            {
//                var temp = mMatchedPairs.Select(x => x.SourceItem).ToList();
//                _unMatchedSource = temp.Except(_SourceList).ToList();
//                return _unMatchedSource;
//            }
//        }

//        private bool mDictBuild
//        {
//            get { return _SourceList.Count == 0 & _MergeList.Count == 0; }
//        }
        

//        //public BuildGenericMergeList(string query, Action<string, dbConnect, List<T>> action)
//        //{
//        //    _action = action;
//        //    if (query.Length > 0)
//        //        BuildList(query);
//        //}

//        private void BuildMatchingPairs(List<PropertyInfo> properties)
//        {
//            foreach (var tLoopVariable in SourceList)
//            {
               
//                var mergeItem = GetMatchingMerge(tLoopVariable, properties);
//                if ((mergeItem == null) == false)
//                    mMatchedPairs.Add(new MatchPair<T>
//                    {
//                        SourceItem = tLoopVariable,
//                        MergeItem = mergeItem
//                    });
//            }
//        }

//        //private List<MatchPair<T>> mMatchedPairs { get; set; }
//        //public List<MatchPair<T>> MatchedPairs
//        //{
//        //    get
//        //    {
//        //        if (mMatchedPairs.Count == 0)
//        //            BuildMatchingPairs(properties);
//        //        return mMatchedPairs;
//        //    }
//        //}

//        private Action<string, dbConnect, List<T>> _action { get; set; }

//        private void BuildList(string query, dbConnect dbConnect, List<T> list)
//        {
//            _action.Invoke(query, dbConnect, list);
//        }

//        private Dictionary<PropertyInfo, PropertyInfo> PropDict { get; set; }


//        private readonly List<Func<T, T, bool>> _rules = new List<Func<T, T, bool>>();
//        public T GetMatchingMerge(T item, List<PropertyInfo> matchProperties)
//    {
//        List<PropertyInfo> getPropList = item.GetType().GetProperties().Where(s => s.Equals(matchProperties.Where(x => x.Name == s.Name).Select(a => a))).ToList();
				
//       // List<PropertyInfo> getPropList = item.GetType().GetProperties().Where(s => false).ToList();
//        if (PropDict.Count < matchProperties.Count)
//            PropDict = getPropList.ToDictionary(p => matchProperties.Where(s => s.Name == p.Name).AsQueryable().FirstOrDefault());

//        if (_rules.Count != matchProperties.Count) {
//            _rules.Clear();
//           // foreach (var pair in PropDict)
//          foreach (var lambda in from pair in PropDict select pair)
//            {
//                PropertyInfo leftProperty = lambda.Key;
//                PropertyInfo rightProperty = lambda.Value;
//                var leftParameter = Expression.Parameter(item.GetType(), item.GetType().Name);
//                var rightParameter = Expression.Parameter(rightProperty.DeclaringType, rightProperty.DeclaringType.Name);
//                var left= Expression.Property(leftParameter, leftProperty);
//                var right = Expression.Property(rightParameter, rightProperty);
//                var leftEqualsRight = Expression.Equal(left, right);
//                _rules.Add(Expression.Lambda<Func<T, T, bool>>(leftEqualsRight, new[] {leftParameter,rightParameter}).Compile());
//              }
//        }

//        var match = Mergelist.FirstOrDefault(r => _rules.All(x => x.Invoke(item, r)));
//        return match;
//    }

//    }

//    public class MatchPair<T> : INamed where T : INamed
//    {
//        public T SourceItem { get; set; }
//        public T MergeItem { get; set; }

//        public override string ToString()
//        {
//            return MergeItem.Name();
//        }
//        string INamed.Name()
//        {
//            return ToString();
//        }

//    }

//    public interface INamed
//    {
//        string Name();
//    }

//    ///Used by calling classes to build a property list.'''

//    static class ReflectionUtility
//    {
//        public static MemberInfo GetMemberInfo<T>(Expression<Func<T>> expression)
//        {
//            var member = expression.Body as MemberExpression;
//            if (member != null)
//                return member.Member;
//            throw new ArgumentException("Expression is not a member", "expression");
//        }
//    }

//}
