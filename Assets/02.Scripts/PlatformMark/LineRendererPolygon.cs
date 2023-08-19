using UnityEngine;

public class LineRendererPolygon : MonoBehaviour
{
	[SerializeField][Range(3, 100)]
	private	int			 polygonPoints = 3;	// 점 개수 (3 ~ 100개)
	[SerializeField][Min(0.1f)]
	private	float		 radius = 3;		// 반지름

	private	LineRenderer lineRenderer;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();

		lineRenderer.loop = true;
	}

	private void Update()
	{
		Play();
	}

	private void Play()
	{
		lineRenderer.positionCount = polygonPoints;

		float anglePerStep = 2 * Mathf.PI * ((float)1/polygonPoints);

		for ( int i = 0; i < polygonPoints; ++ i )
		{
			Vector2	point = Vector2.zero;
			float	angle = anglePerStep * i;
			
			point.x = Mathf.Cos(angle) * radius;
			point.y = Mathf.Sin(angle) * radius;

			lineRenderer.SetPosition(i, point);
		}
	}
}

