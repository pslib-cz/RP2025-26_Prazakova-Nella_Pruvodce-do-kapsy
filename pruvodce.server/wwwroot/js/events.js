window.toggleEvent = function(eventId) {
    const content = document.getElementById(`content-${eventId}`);
    const header = document.querySelector(`[data-event-id="${eventId}"] .admin-accordion__header`);
    const toggle = header?.querySelector('.admin-accordion__toggle');

    if (!content) return;

    content.classList.toggle('open');
    if (toggle) {
        toggle.style.transform = content.classList.contains('open') ? 'rotate(180deg)' : 'rotate(0deg)';
    }
};

// Toggle event active status (checkbox)
window.toggleEventActive = function(eventId, isActive, checkbox) {
    if (isActive) {
        // Zkontroluj, zda existuje jiná aktivní akce
        fetch(`./Events/Index?handler=GetActiveEventCount&excludeEventId=${eventId}`)
            .then(r => r.json())
            .then(data => {
                if (data.activeCount > 0) {
                    const confirmed = confirm(
                        'Pozor! Jedna nebo více aktivních akcí už existuje.\n\n' +
                        'Jste si jisti, že chcete zobrazit obě akce na mapě zároveň?\n\n' +
                        'Zkontrolujte seznam akcí a deaktivujte všechny akce, které aktivní být nemají.'
                    );
                    if (!confirmed) {
                        checkbox.checked = false;
                        return;
                    }
                }
                submitToggleForm(eventId, isActive);
            })
            .catch(() => submitToggleForm(eventId, isActive));
    } else {
        submitToggleForm(eventId, isActive);
    }
};

function submitToggleForm(eventId, isActive) {
    const form = document.getElementById(`toggle-form-${eventId}`);
    const hiddenInput = document.getElementById(`isactive-${eventId}`);
    hiddenInput.value = isActive ? 'true' : 'false';
    form.submit();
}

// ===== MODAL PRO STANOVIŠTĚ =====

let currentEventId = null;

// Otevřít modal pro přidání stanovišť
window.openAddPointsModal = function(eventId) {
    currentEventId = eventId;
    document.getElementById('modalEventId').value = eventId;
    document.getElementById('pointsModal').classList.add('open');

    fetch(`./Events/Index?handler=GetAvailablePoints&eventId=${eventId}`)
        .then(response => {
            if (!response.ok) throw new Error('Chyba při načítání');
            return response.json();
        })
        .then(data => {
            const body = document.getElementById('pointsModalBody');
            if (!data.points || data.points.length === 0) {
                body.innerHTML = '<p class="admin-loading">Všechna dostupná stanoviště jsou již přidána.</p>';
            } else {
                body.innerHTML = data.points.map(p => `
                    <label class="admin-modal__item">
                        <input type="checkbox" name="PointIds" value="${escapeHtml(p.id)}">
                        <div class="admin-modal__item-info">
                            <p class="admin-modal__item-name">${escapeHtml(p.name)}</p>
                            <p class="admin-modal__item-code">${escapeHtml(p.specialization || '')}</p>
                        </div>
                    </label>
                `).join('');
            }
        })
        .catch(error => {
            console.error('Chyba:', error);
            document.getElementById('pointsModalBody').innerHTML = '<p class="admin-loading" style="color: red;">Chyba při načítání stanovišť</p>';
        });
};

window.closePointsModal = function() {
    document.getElementById('pointsModal').classList.remove('open');
    currentEventId = null;
};

function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

document.addEventListener('DOMContentLoaded', function() {
    const pointsModal = document.getElementById('pointsModal');

    if (pointsModal) {
        pointsModal.addEventListener('click', function(e) {
            if (e.target === this) closePointsModal();
        });
    }
});