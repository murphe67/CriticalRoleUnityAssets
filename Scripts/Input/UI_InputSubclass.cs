using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriticalRole.Map;

namespace CriticalRole.UI
{
    public interface UI_InputSubclass
    {
        void IHexagonClicked(IHexagon hexagon);

        void IHexagonHovered(IHexagon hexagon);

        void IHexagonUnhovered(IHexagon hexagon);

        void BackButton();
    }
}

