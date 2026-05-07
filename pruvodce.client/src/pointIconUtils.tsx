export type PointIconKey = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';

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
  Jine: 'lucide:map-pin',
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