using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class NearExitTriggerScript : MonoBehaviour
{
	// Token: 0x06000995 RID: 2453 RVA: 0x00024288 File Offset: 0x00022688
	private void OnTriggerEnter(Collider other)
	{
		if ((this.gc.exitsReached < 3 || gc.mode == "trull") && this.gc.finaleMode && other.tag == "Player")
		{
			Debug.Log(this.gc.exitsReached);
            this.gc.ExitReached(transform ,id);
            this.es.Lower();
            if (gc.mode == "trull")
			{
				if (this.gc.exitsReached < 2)
                    if (this.gc.baldiScrpt.isActiveAndEnabled) this.gc.baldiScrpt.Hear(base.transform.position, 8f);
            }
            else
				if (this.gc.baldiScrpt.isActiveAndEnabled) this.gc.baldiScrpt.Hear(base.transform.position, 8f);
        }
	}

	// Token: 0x04000674 RID: 1652
	public GameControllerScript gc;

	// Token: 0x04000675 RID: 1653
	public EntranceScript es;
	public int id;
}
