using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meta {
    /// <summary>
    /// A silly pojo meant to allow you to add comments to gameobjects in inspector
    /// </summary>
    public class Comment : MonoBehaviour {
        //This is the text of the comment
        [TextArea] public string notes = "Comment here.";
    }
}
