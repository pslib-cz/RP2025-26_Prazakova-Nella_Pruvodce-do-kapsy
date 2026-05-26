import type { Point } from './Types/MapType';

export type PointIconKey = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';
export type FieldTypeKey = 'IT' | 'EL' | 'ST' | 'TL' | 'OD' | 'TE';

const pointIconByNumber: Record<number, PointIconKey> = {
  0: 'Talk',
  1: 'Hand',
  2: 'Ucebna',
  3: 'Jine',
};

export const pointIconByType: Record<PointIconKey, string> = {
  Talk: 'lucide:presentation',
  Hand: 'lucide:hand',
  Ucebna: 'lucide:door-open',
  Jine: 'lucide--ellipsis',
};

export const pointIconLabel: Record<PointIconKey, string> = {
  Talk: 'Přednáška',
  Hand: 'Praktické stanoviště',
  Ucebna: 'Ukázka učebny',
  Jine: 'Jiné',
};

export function getPointIconKey(icon: unknown): PointIconKey {
  if (typeof icon === 'number') {
    return pointIconByNumber[icon] ?? 'Jine';
  }

  if (typeof icon === 'string') {
    const normalized = icon.trim();

    const asNumber = Number(normalized);
    if (!Number.isNaN(asNumber)) {
      return pointIconByNumber[asNumber] ?? 'Jine';
    }

    if (normalized === 'Talk') return 'Talk';
    if (normalized === 'Hand') return 'Hand';
    if (normalized === 'Ucebna') return 'Ucebna';
    if (normalized === 'Jine') return 'Jine';
  }

  return 'Jine';
}

export function getPointIconName(icon: unknown): string {
  const key = getPointIconKey(icon);
  return pointIconByType[key];
}

export function getPointIconLabel(icon: unknown): string {
  const key = getPointIconKey(icon);
  return pointIconLabel[key];
}

const fieldTypeByNumber: Record<number, FieldTypeKey> = {
  0: 'IT',
  1: 'EL',
  2: 'ST',
  3: 'TL',
  4: 'OD',
  5: 'TE',
};

export const fieldTypeLabel: Record<FieldTypeKey, string> = {
  IT: 'Informační technologie',
  EL: 'Elektrotechnika',
  ST: 'Strojírenství',
  TL: 'Technické lyceum',
  OD: 'Oděvnictví',
  TE: 'Textilnictví',
};

export function getFieldTypeKey(type: unknown): FieldTypeKey | null {
  if (typeof type === 'number') {
    return fieldTypeByNumber[type] ?? null;
  }

  if (typeof type === 'string') {
    const normalized = type.trim().toUpperCase();

    const asNumber = Number(normalized);
    if (!Number.isNaN(asNumber)) {
      return fieldTypeByNumber[asNumber] ?? null;
    }

    if (normalized === 'IT') return 'IT';
    if (normalized === 'EL') return 'EL';
    if (normalized === 'ST') return 'ST';
    if (normalized === 'TL') return 'TL';
    if (normalized === 'OD') return 'OD';
    if (normalized === 'TE') return 'TE';
  }

  return null;
}

export function getFieldTypeLabel(type: unknown): string {
  const key = getFieldTypeKey(type);

  if (!key) {
    return 'Bez oboru';
  }

  return fieldTypeLabel[key];
}

export function getFieldThemeClass(type: unknown): string {
  const key = getFieldTypeKey(type);

  switch (key) {
    case 'IT':
      return 'themeIT';
    case 'EL':
      return 'themeEL';
    case 'ST':
      return 'themeST';
    case 'TL':
      return 'themeTL';
    case 'OD':
      return 'themeOD';
    case 'TE':
      return 'themeTE';
    default:
      return 'themeDefault';
  }
}

export function getFieldPointClass(type: unknown, styles: Record<string, string>): string {
  const key = getFieldTypeKey(type);

  switch (key) {
    case 'IT':
      return styles.typeIT;
    case 'EL':
      return styles.typeEL;
    case 'ST':
      return styles.typeST;
    case 'TL':
      return styles.typeTL;
    case 'OD':
      return styles.typeOD;
    case 'TE':
      return styles.typeTE;
    default:
      return styles.typeDefault;
  }
}

export function getPointTitle(point: Point): string {
  const rawPoint = point as Point & {
    label?: string | null;
    name?: string | null;
    title?: string | null;
  };

  return rawPoint.label ?? rawPoint.name ?? rawPoint.title ?? 'Stanoviště';
}

export function getPointDescription(point: Point): string {
  const rawPoint = point as Point & {
    description?: string | null;
  };

  return rawPoint.description ?? '';
}

export function getPointRoomLabel(point: Point): string {
  const rawPoint = point as Point & {
    roomLabel?: string | null;
    roomCode?: string | null;
    roomId?: string | null;
  };

  return rawPoint.roomLabel ?? rawPoint.roomCode ?? rawPoint.roomId ?? 'místnost';
}

export function getPointSpecialization(point: Point) {
  const rawPoint = point as Point & {
    specialization?: {
      name?: string | null;
      description?: string | null;
      type?: string | number | null;
      Type?: string | number | null;
    } | null;
  };

  return rawPoint.specialization ?? null;
}

export function getPointFieldType(point: Point): FieldTypeKey | null {
  const rawPoint = point as Point & {
    fieldType?: string | number | null;
    specialization?: {
      type?: string | number | null;
      Type?: string | number | null;
    } | null;
  };

  return getFieldTypeKey(
    rawPoint.fieldType ??
      rawPoint.specialization?.type ??
      rawPoint.specialization?.Type
  );
}

export function getPointFieldLabel(point: Point): string {
  const specialization = getPointSpecialization(point);

  if (specialization?.name) {
    return specialization.name;
  }

  return getFieldTypeLabel(getPointFieldType(point));
}

export function getPointThemeClass(point: Point): string {
  return getFieldThemeClass(getPointFieldType(point));
}

export function getPointMapColorClass(
  point: Point,
  styles: Record<string, string>
): string {
  return getFieldPointClass(getPointFieldType(point), styles);
}

export function getPointIconFromPoint(point: Point): string {
  const rawPoint = point as Point & {
    icon?: string | number | null;
    Icon?: string | number | null;
  };

  return getPointIconName(rawPoint.icon ?? rawPoint.Icon);
}