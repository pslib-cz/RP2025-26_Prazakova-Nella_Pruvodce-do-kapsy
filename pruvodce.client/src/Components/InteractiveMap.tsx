import React, { useEffect, useMemo, useState } from 'react';
import { TransformWrapper, TransformComponent } from 'react-zoom-pan-pinch';
import { Icon } from '@iconify/react';

import BackgroundM from './BackgroundM';
import BackgroundT from './BackgroundT';
import InteractivePoint from './Point';

import { RoomType, type FloorData, type Point } from '../Types/MapType';

const roomColors: Record<number, { bg: string; hover: string; border: string }> = {
  [RoomType.Classroom]: {
    bg: 'url(#gradient-classroom)',
    hover: '#c1c1c1',
    border: '#50555A',
  },
  [RoomType.Specialized]: {
    bg: 'url(#gradient-specialized)',
    hover: 'rgba(192, 192, 192, 0.7)',
    border: '#50555A',
  },
  [RoomType.Office]: {
    bg: 'url(#gradient-office)',
    hover: '#9b9d9e',
    border: '#50555A',
  },
  [RoomType.Toilets]: {
    bg: 'url(#gradient-wc)',
    hover: '#99b4cd',
    border: '#50555A',
  },
  [RoomType.Elevator]: {
    bg: 'url(#gradient-elevator)',
    hover: '#A3735C',
    border: '#744935',
  },
  [RoomType.Other]: {
    bg: 'url(#gradient-other)',
    hover: '#606366',
    border: '#50555A',
  },
};

interface InteractiveMapProps {
  floors: FloorData[];
  activeFloorId: number;
  buildingId: number;
  className?: string;
  onPointSelect?: (point: Point) => void;
  onMapClick?: () => void;
}

function useIsDesktop(): boolean {
  const [isDesktop, setIsDesktop] = useState(() => {
    if (typeof window === 'undefined') return true;
    return window.matchMedia('(min-width: 1024px)').matches;
  });

  useEffect(() => {
    if (typeof window === 'undefined') return;

    const mediaQuery = window.matchMedia('(min-width: 1024px)');

    const handleChange = () => {
      setIsDesktop(mediaQuery.matches);
    };

    handleChange();
    mediaQuery.addEventListener('change', handleChange);

    return () => {
      mediaQuery.removeEventListener('change', handleChange);
    };
  }, []);

  return isDesktop;
}

function getPointCoordinate(
  point: Point,
  fallbackX: number,
  fallbackY: number
): { x: number; y: number } {
  const rawPoint = point as Point & {
    coordinateX?: number;
    coordinateY?: number;
    x?: number;
    y?: number;
  };

  return {
    x: rawPoint.coordinateX ?? rawPoint.x ?? fallbackX,
    y: rawPoint.coordinateY ?? rawPoint.y ?? fallbackY,
  };
}

const InteractiveMap: React.FC<InteractiveMapProps> = ({
  floors,
  activeFloorId,
  buildingId,
  className,
  onPointSelect,
  onMapClick,
}) => {
  const [zoomLevel, setZoomLevel] = useState(1);
  const isDesktop = useIsDesktop();

  const activeFloorData = useMemo(() => {
    return floors.find(floor => floor.floorId === activeFloorId) ?? floors[0];
  }, [floors, activeFloorId]);

  const backgroundMap: Record<number, React.FC<{ zoomLevel: number }>> = {
    1: BackgroundM,
    2: BackgroundT,
  };

  const SelectedBackground = backgroundMap[buildingId] ?? BackgroundM;

  const pointRenderData = useMemo(() => {
    if (!activeFloorData) return [];

    return activeFloorData.rooms.flatMap(room => {
      const roomPoints = room.points ?? [];

      return roomPoints.map((point, index) => {
        const fallbackX = room.coordinateX ?? 0;
        const fallbackY = room.coordinateY ?? 0;

        const pointBaseCoordinate = getPointCoordinate(point, fallbackX, fallbackY);

        const hasOwnCoordinates =
          (point as Point & { coordinateX?: number; coordinateY?: number }).coordinateX != null ||
          (point as Point & { x?: number; y?: number }).x != null;

        if (hasOwnCoordinates) {
          return {
            point,
            x: pointBaseCoordinate.x,
            y: pointBaseCoordinate.y,
          };
        }

        const offsetStep = 14;
        const offset = index * offsetStep;

        return {
          point,
          x: pointBaseCoordinate.x + offset,
          y: pointBaseCoordinate.y,
        };
      });
    });
  }, [activeFloorData]);

  if (!activeFloorData) {
    return <div>Patro nebylo nalezeno.</div>;
  }

  const initialScale = isDesktop ? 0.6 : 0.95;
  const minScale = isDesktop ? 0.5 : 0.8;
  const initialPositionX = isDesktop ? 160 : 0;
  const initialPositionY = isDesktop ? -120 : 0;

  return (
    <div
      className={`map-wrapper ${className ?? ''}`}
      onClick={onMapClick}
      style={{
        position: 'relative',
        width: '100%',
        height: '100%',
        overflow: 'hidden',
      }}
    >
      <TransformWrapper
        key={`${buildingId}-${activeFloorId}-${isDesktop ? 'desktop' : 'mobile'}`}
        onTransform={ref => setZoomLevel(ref.state.scale)}
        initialScale={initialScale}
        minScale={minScale}
        maxScale={4.5}
        initialPositionX={initialPositionX}
        initialPositionY={initialPositionY}
        limitToBounds
        centerOnInit={!isDesktop}
        wheel={{ step: 0.12 }}
        doubleClick={{ disabled: true }}
        panning={{ velocityDisabled: true }}
      >
        <TransformComponent
          wrapperStyle={{
            width: '100%',
            height: '100%',
            backgroundColor: '#EAEAEA',
          }}
          contentStyle={{
            width: isDesktop ? '80vw' : '100vw',
            height: isDesktop ? '220vh' : '100vh',
          }}
        >
          <svg
            viewBox="0 0 540 900"
            preserveAspectRatio="xMidYMid slice"
            style={{
              width: '100%',
              height: '100%',
              display: 'block',
            }}
          >
            <defs>
              <radialGradient
                id="gradient-classroom"
                cx="0"
                cy="0"
                r="1"
                gradientUnits="userSpaceOnUse"
                gradientTransform="translate(22 50) rotate(66.2505) scale(54.626 54.626)"
              >
                <stop stopColor="#E5E4E2" />
                <stop offset="1" stopColor="#CDCDCD" />
              </radialGradient>

              <radialGradient
                id="gradient-specialized"
                cx="0"
                cy="0"
                r="1"
                gradientUnits="userSpaceOnUse"
                gradientTransform="translate(22 50) rotate(66.2505) scale(54.626 54.626)"
              >
                <stop stopColor="#D8D8D8" />
                <stop offset="1" stopColor="#C0C0C0" />
              </radialGradient>

              <radialGradient
                id="gradient-office"
                cx="0"
                cy="0"
                r="1"
                gradientTransform="matrix(-9.04412 14.4189 -10.6985 -12.1892 9.15441 18.7297)"
                gradientUnits="userSpaceOnUse"
              >
                <stop stopColor="#C6C4C1" />
                <stop offset="1" stopColor="#AAAAAA" />
              </radialGradient>

              <radialGradient
                id="gradient-elevator"
                cx="0"
                cy="0"
                r="1"
                gradientUnits="userSpaceOnUse"
                gradientTransform="translate(7.5 23) rotate(90) scale(23 7.5)"
              >
                <stop stopColor="#C58D73" />
                <stop offset="1" stopColor="#B87B5E" />
              </radialGradient>

              <radialGradient
                id="gradient-wc"
                cx="0"
                cy="0"
                r="1"
                gradientUnits="userSpaceOnUse"
                gradientTransform="translate(10 22.5) rotate(90) scale(31.1538 8.85246)"
              >
                <stop stopColor="#C4D4E1" />
                <stop offset="1" stopColor="#B5C6D4" />
              </radialGradient>

              <radialGradient
                id="gradient-other"
                cx="0"
                cy="0"
                r="1"
                gradientTransform="matrix(16.5 12 -16.5 9.51398 16.5 12)"
                gradientUnits="userSpaceOnUse"
              >
                <stop stopColor="#7C8083" />
                <stop offset="1" stopColor="#6B7074" />
              </radialGradient>
            </defs>

            {!activeFloorData.backgroundUrl && <SelectedBackground zoomLevel={zoomLevel} />}

            {activeFloorData.backgroundUrl && (
              <image href={activeFloorData.backgroundUrl} x="0" y="0" width="540" height="900" />
            )}

            <g id="rooms-layer">
              {activeFloorData.rooms.map(room => {
                const pathData = room.svgOutline || room.svgData;

                if (!pathData) return null;

                const rawRoomType = Number(room.type);
                const safeRoomType = Number.isNaN(rawRoomType) ? RoomType.Other : rawRoomType;
                const colors = roomColors[safeRoomType] ?? roomColors[RoomType.Other];

                return (
                  <path
                    key={room.roomId}
                    d={pathData}
                    fill={colors.bg}
                    stroke={colors.border}
                    strokeWidth={1 / zoomLevel}
                    onClick={event => {
                      event.stopPropagation();
                      onMapClick?.();
                    }}
                    style={{
                      cursor: 'pointer',
                      transition: 'fill 0.2s',
                      pointerEvents: 'all',
                    }}
                    onMouseEnter={event => {
                      event.currentTarget.style.fill = colors.hover;
                    }}
                    onMouseLeave={event => {
                      event.currentTarget.style.fill = colors.bg;
                    }}
                  />
                );
              })}
            </g>

            <g id="room-labels-layer">
              {activeFloorData.rooms.map(room => {
                const rawRoomType = Number(room.type);
                const safeRoomType = Number.isNaN(rawRoomType) ? RoomType.Other : rawRoomType;

                const isToilets = safeRoomType === RoomType.Toilets;
                const isElevator = safeRoomType === RoomType.Elevator;
                const displayLabel = room.label?.trim();

                if (
                  zoomLevel <= 1.5 ||
                  room.coordinateX == null ||
                  room.coordinateY == null
                ) {
                  return null;
                }

                if (room.icon) {
                  return (
                    <foreignObject
                      key={room.roomId}
                      x={room.coordinateX - 10 / zoomLevel}
                      y={room.coordinateY - 10 / zoomLevel}
                      width={20 / zoomLevel}
                      height={20 / zoomLevel}
                      style={{ pointerEvents: 'none' }}
                    >
                      <div
                        style={{
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                          width: '100%',
                          height: '100%',
                        }}
                      >
                        <Icon
                          icon={room.icon}
                          style={{
                            width: '100%',
                            height: '100%',
                            color: isToilets
                              ? '#647F97'
                              : isElevator
                                ? '#89482A'
                                : '#25292c',
                          }}
                        />
                      </div>
                    </foreignObject>
                  );
                }

                if (!displayLabel) return null;

                return (
                  <text
                    key={room.roomId}
                    x={room.coordinateX}
                    y={room.coordinateY}
                    textAnchor="middle"
                    dominantBaseline="middle"
                    style={{
                      fontSize: `${12 / zoomLevel}px`,
                      fill: '#333',
                      fontWeight: 'bold',
                      pointerEvents: 'none',
                      userSelect: 'none',
                    }}
                  >
                    {displayLabel}
                  </text>
                );
              })}
            </g>

            {activeFloorData.detailUrl && (
              <image
                href={activeFloorData.detailUrl}
                x="0"
                y="0"
                width="540"
                height="900"
                style={{ pointerEvents: 'none' }}
              />
            )}

            <g id="points-layer">
              {pointRenderData.map(({ point, x, y }) => (
                <InteractivePoint
                  key={point.pointId}
                  point={point}
                  x={x}
                  y={y}
                  zoomLevel={zoomLevel}
                  onClick={selected => {
                    onPointSelect?.(selected);
                  }}
                />
              ))}
            </g>
          </svg>
        </TransformComponent>
      </TransformWrapper>
    </div>
  );
};

export default InteractiveMap;