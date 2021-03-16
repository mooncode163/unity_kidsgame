using UnityEngine;

public class SVGPathSegMovetoAbs : SVGPathSeg, ISVGDrawableSeg {
  private float _x = 0f, _y = 0f;

  public float x { get { return this._x; } }

  public float y { get { return this._y; } }

  public SVGPathSegMovetoAbs(float x, float y) : base() {
    this._x = x;
    this._y = y;
  }

  public override Vector2 currentPoint { get { return new Vector2(this._x, this._y); } }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddMoveTo(new Vector2(this._x, this._y));
  }
}
