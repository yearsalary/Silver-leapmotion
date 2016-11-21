using UnityEngine;
using System.Collections;

public class PlayRecordData {
    public string trainee_id;
    public string assistant_id;
    public string contents_name;
    public string level;
    public string totalScore;
    public string startTime;
    public string endTime;

	public PlayRecordData(string trainee_id, string assistant_id, string contents_name, string level, string totalScore, string startTime, string endTime) {
		this.trainee_id = trainee_id;
		this.assistant_id = assistant_id;
		this.contents_name = contents_name;
		this.level = level;
		this.startTime = startTime;
		this.endTime = endTime;
	}

}
