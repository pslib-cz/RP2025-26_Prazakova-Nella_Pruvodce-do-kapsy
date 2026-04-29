import React, { useState } from 'react';
import { TransformWrapper, TransformComponent } from 'react-zoom-pan-pinch';
import BackgroundM from './BackgroundM';
import BackgroundT from './BackgroundT';
import { Icon } from '@iconify/react';
import { RoomType, type FloorData, type Point } from '../Types/MapType';
import InteractivePoint from './Point';

const roomColors: Record<number, { bg: string; hover: string; border: string }> = {
  [RoomType.Classroom]: {
    bg: 'url(#gradient-classroom)',
    hover: 'rgba(205, 205, 205, 0.7)',
    border: '#50555A'
  },
  [RoomType.Specialized]: {
    bg: 'url(#gradient-specialized)',
    hover: 'rgba(192, 192, 192, 0.7)',
    border: '#50555A'
  },
  [RoomType.Office]: {
    bg: 'url(#gradient-office)',
    hover: 'rgba(170, 170, 170, 0.7)',
    border: '#50555A'
  },
  [RoomType.Toilets]: {
    bg: 'url(#gradient-wc)',
    hover: 'rgba(181, 198, 212, 0.7)',
    border: '#50555A'
  },
  [RoomType.Elevator]: {
    bg: 'url(#gradient-elevator)',
    hover: 'rgba(107, 112, 116, 0.7)',
    border: '#50555A'
  },
  [RoomType.Other]: {
    bg: 'url(#gradient-other)',
    hover: 'rgba(200, 200, 200, 0.7)',
    border: '#50555A'
  }
};

interface InteractiveMapProps {
  floors: FloorData[];
  activeFloorId: number;
  buildingId: number;
  className?: string;
}

const InteractiveMap: React.FC<InteractiveMapProps> = ({
  floors,
  activeFloorId,
  buildingId,
  className
}) => {
  const [selectedPoint, setSelectedPoint] = useState<Point | null>(null);
  const [zoomLevel, setZoomLevel] = useState(1);

  const isDesktop = window.innerWidth >= 1024;
  const activeFloorData = floors.find(f => f.floorId === activeFloorId) ?? floors[0];

  const backgroundMap: Record<number, React.FC<{ zoomLevel: number }>> = {
    1: BackgroundM,
    2: BackgroundT
  };

  const SelectedBackground = backgroundMap[buildingId] ?? BackgroundM;

  if (!activeFloorData) {
    return <div>Patro nebylo nalezeno.</div>;
  }

  return (
    <div
      className={`map-wrapper ${className || ''}`}
      style={{ position: 'relative' }}
    >
      {selectedPoint && (
        <div
          style={{
            position: 'absolute',
            top: 16,
            left: 16,
            zIndex: 10,
            background: 'white',
            padding: '8px 12px',
            borderRadius: 8,
            boxShadow: '0 2px 8px rgba(0,0,0,0.15)',
            maxWidth: 280
          }}
        >
          <button
            type="button"
            onClick={() => setSelectedPoint(null)}
            style={{
              float: 'right',
              border: 'none',
              background: 'transparent',
              cursor: 'pointer',
              fontSize: 16
            }}
            aria-label="Zavřít detail"
          >
            ×
          </button>

          <strong>{selectedPoint.label}</strong>

          {selectedPoint.description && <div>{selectedPoint.description}</div>}

          {selectedPoint.event && (
            <div>
              <small>Event: {selectedPoint.event.name}</small>
            </div>
          )}

          {selectedPoint.specialization && (
            <div>
              <small>Specializace: {selectedPoint.specialization.name}</small>
            </div>
          )}

          {selectedPoint.teachers && selectedPoint.teachers.length > 0 && (
            <div>
              <small>
                Učitelé:{' '}
                {selectedPoint.teachers
                  .map(t => `${t.firstN} ${t.lastN}`)
                  .join(', ')}
              </small>
            </div>
          )}

          {selectedPoint.subjects && selectedPoint.subjects.length > 0 && (
            <div>
              <small>
                Předměty:{' '}
                {selectedPoint.subjects
                  .map(s => s.acronym || s.name)
                  .join(', ')}
              </small>
            </div>
          )}
        </div>
      )}

      <TransformWrapper
        onTransform={(ref) => setZoomLevel(ref.state.scale)}
        initialScale={isDesktop ? 0.6 : 0.95}
        minScale={isDesktop ? 0.5 : 0.8}
        initialPositionX={isDesktop ? 160 : 0}
        initialPositionY={isDesktop ? -120 : 0}
        limitToBounds={true}
      >
        <TransformComponent
          wrapperStyle={{
            width: '100vw',
            height: '100vh',
            backgroundColor: '#EAEAEA'
          }}
          contentStyle={{
            width: isDesktop ? '80vw' : '100vw',
            height: isDesktop ? '220vh' : '100vh'
          }}
        >
          <svg
            viewBox="0 0 540 900"
            preserveAspectRatio="xMidYMid slice"
            style={{ width: '100%', height: '100%', display: 'block' }}
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

            {!activeFloorData.backgroundUrl && (
              <SelectedBackground zoomLevel={zoomLevel} />
            )}

            {activeFloorData.backgroundUrl && (
              <image
                href={activeFloorData.backgroundUrl}
                x="0"
                y="0"
                width="540"
                height="900"
              />
            )}

            {activeFloorData.rooms.map(room => {
              const pathData = room.svgOutline || room.svgData;

              if (!pathData) {
                return null;
              }

              const rawRoomType = Number(room.type);
              const safeRoomType = Number.isNaN(rawRoomType)
                ? RoomType.Other
                : rawRoomType;

              const colors = roomColors[safeRoomType] ?? roomColors[RoomType.Other];

              const isToilets = safeRoomType === RoomType.Toilets;
              const isElevator = safeRoomType === RoomType.Elevator;

              return (
                <React.Fragment key={room.roomId}>
                  <path
                    d={pathData}
                    fill={colors.bg}
                    stroke={colors.border}
                    strokeWidth={1 / zoomLevel}
                    onClick={() => {
                      console.log('Kliknuto na místnost:', room.label || room.roomId);
                    }}
                    style={{
                      cursor: 'pointer',
                      transition: 'fill 0.2s',
                      pointerEvents: 'all'
                    }}
                    onMouseEnter={(e) => {
                      e.currentTarget.style.fill = colors.hover;
                    }}
                    onMouseLeave={(e) => {
                      e.currentTarget.style.fill = colors.bg;
                    }}
                  />

                  {zoomLevel > 2 && room.interiorImageUrl && (
                    <image
                      href={room.interiorImageUrl}
                      x={room.interiorX ?? 0}
                      y={room.interiorY ?? 0}
                      width={room.interiorWidth ?? 0}
                      height={room.interiorHeight ?? 0}
                      style={{ pointerEvents: 'none' }}
                    />
                  )}

                  {zoomLevel > 1.5 &&
                    room.coordinateX != null &&
                    room.coordinateY != null &&
                    (room.icon ? (
                      <foreignObject
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
                            height: '100%'
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
                                  : '#25292c'
                            }}
                          />
                        </div>
                      </foreignObject>
                    ) : (
                      room.label && (
                        <text
                          x={room.coordinateX}
                          y={room.coordinateY}
                          textAnchor="middle"
                          dominantBaseline="middle"
                          style={{
                            fontSize: `${12 / zoomLevel}px`,
                            fill: '#333',
                            fontWeight: 'bold',
                            pointerEvents: 'none',
                            userSelect: 'none'
                          }}
                        >
                          {room.label}
                        </text>
                      )
                    ))}

                  {room.points?.map((point, index) => (
                    <InteractivePoint
                      key={point.pointId}
                      point={point}
                      x={(room.coordinateX ?? 0) + index * (14 / zoomLevel)}
                      y={(room.coordinateY ?? 0) - 18 / zoomLevel}
                      zoomLevel={zoomLevel}
                      onClick={setSelectedPoint}
                    />
                  ))}
                </React.Fragment>
              );
            })}

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
          </svg>
        </TransformComponent>
      </TransformWrapper>
    </div>
  );
};

export default InteractiveMap;