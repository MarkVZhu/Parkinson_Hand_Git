using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
	public int capacity;
	public RowManager _rowManager;
	public RectTransform Content;
	// Start is called before the first frame update
	void Awake() {
		Content = (RectTransform)transform.parent;
	}
	
	private void OnEnable() {
		printScore(PlayerMessageManager.Instance.currentRankList);
	}
	
	void OnDisable() {
		for (int i = 0; i < transform.childCount; i++) {  
			Destroy (transform.GetChild (i).gameObject);  
		}  
	}
	
	public void printScore(List<PlayerMessage> list)
	{
		if(list.Count<capacity)
		{
			Content.sizeDelta = new Vector2(Content.sizeDelta.x, list.Count * 120);
			for (int i = 0; i < list.Count; i++)
			{
				var row = Instantiate(_rowManager, transform).GetComponent<RowManager>();
				row.Rank.text = "  "+(i+1);
				row.Count.text = list[i].number.ToString();
				row.Time.text = list[i].time.ToString();
				row.Frequency.text = string.Format("{0:F1}",list[i].avFrequency);
				row.Range.text = string.Format("{0:P1}",list[i].avRange);
				row.Score.text = list[i].score.ToString();
			}
		}
		else
		{
			Content.sizeDelta = new Vector2(Content.sizeDelta.x, capacity * 120);
			for (int i = 0; i < capacity; i++)
			{
				var row = Instantiate(_rowManager, transform).GetComponent<RowManager>();
				row.Rank.text = "  "+(i+1);
				row.Count.text = list[i].number.ToString();
				row.Time.text = list[i].time.ToString();
				row.Frequency.text = string.Format("{0:F1}",list[i].avFrequency);
				row.Range.text = string.Format("{0:P1}",list[i].avRange);
				row.Score.text = list[i].score.ToString();
			}
		}
	}
}
