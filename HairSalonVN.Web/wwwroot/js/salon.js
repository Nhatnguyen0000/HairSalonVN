// ================================================================
// HairSalonVN - Main JavaScript
// Premium Hair Salon Booking System
// ================================================================

/* ── Utility Functions ──────────────────────────────────────────── */
const $ = (sel, ctx = document) => ctx.querySelector(sel);
const $$ = (sel, ctx = document) => [...ctx.querySelectorAll(sel)];

/* ── Auto-dismiss alerts ──────────────────────────────────────── */
$$(".salon-alert").forEach(el => {
    setTimeout(() => {
        el.style.transition = "opacity .5s, transform .5s";
        el.style.opacity = "0";
        el.style.transform = "translateY(-10px)";
        setTimeout(() => el.remove(), 500);
    }, 5000);
});

/* ── Mobile Navigation ────────────────────────────────────────── */
const hamburger = $("#hamburger");
const navLinks = $("#navLinks");
const mobileOverlay = $("#mobileOverlay");

if (hamburger && navLinks) {
    hamburger.addEventListener("click", () => {
        hamburger.classList.toggle("active");
        navLinks.classList.toggle("open");
        document.body.classList.toggle("menu-open");
        hamburger.setAttribute("aria-expanded", navLinks.classList.contains("open"));
        
        if (navLinks.classList.contains("open")) {
            hamburger.setAttribute("aria-label", "Đóng menu");
        } else {
            hamburger.setAttribute("aria-label", "Mở menu");
        }
    });

    // Close on link click
    $$(".nav-links a").forEach(link => {
        link.addEventListener("click", () => {
            hamburger.classList.remove("active");
            navLinks.classList.remove("open");
            document.body.classList.remove("menu-open");
            hamburger.setAttribute("aria-expanded", "false");
        });
    });

    // Close on overlay click
    if (mobileOverlay) {
        mobileOverlay.addEventListener("click", () => {
            hamburger.classList.remove("active");
            navLinks.classList.remove("open");
            document.body.classList.remove("menu-open");
            hamburger.setAttribute("aria-expanded", "false");
        });
    }

    // Close on Escape key
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && navLinks.classList.contains("open")) {
            hamburger.classList.remove("active");
            navLinks.classList.remove("open");
            document.body.classList.remove("menu-open");
            hamburger.setAttribute("aria-expanded", "false");
            hamburger.focus();
        }
    });
}

/* ── Navbar Scroll Effect ─────────────────────────────────────── */
const mainNav = $("#mainNav");
if (mainNav) {
    window.addEventListener("scroll", () => {
        if (window.scrollY > 50) {
            mainNav.classList.add("scrolled");
        } else {
            mainNav.classList.remove("scrolled");
        }
    });
}

/* ── Slot Selection ──────────────────────────────────────────── */
function initSlotSelection() {
    $$("#slotContainer .slot-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            $$("#slotContainer .slot-btn").forEach(b => b.classList.remove("selected"));
            this.classList.add("selected");
            const iso = this.dataset.iso;
            const hdnDate = $("#hdnAppointmentDate");
            if (hdnDate) hdnDate.value = iso;
            const btnNext = $("#btnConfirm");
            if (btnNext) {
                btnNext.removeAttribute("disabled");
                btnNext.classList.add("animate-pulse");
            }
        });
    });
}

/* ── Staff Card Selection ───────────────────────────────────── */
let _currentSlotAbort = null;

function initStaffSelection() {
    $$(".staff-card").forEach(card => {
        card.addEventListener("click", async function () {
            $$(".staff-card").forEach(c => c.classList.remove("selected"));
            this.classList.add("selected");

            const staffId = this.dataset.staffId;
            const serviceId = $("#hdnServiceId")?.value;
            const date = $("#datePicker")?.value;
            const hdnStaff = $("#hdnStaffId");
            if (hdnStaff) hdnStaff.value = staffId;

            if (serviceId && date) {
                if (_currentSlotAbort) _currentSlotAbort.abort();
                const ctrl = new AbortController();
                _currentSlotAbort = ctrl;
                await loadSlots(staffId, serviceId, date, ctrl.signal);
            }
        });
    });
}

/* ── Load Slots via AJAX ─────────────────────────────────────── */
async function loadSlots(staffId, serviceId, date, signal) {
    const container = $("#slotContainer");
    if (!container) return;

    container.innerHTML = `
        <div class="slots-loading">
            <div class="loading-spinner"></div>
            <p>Đang tải khung giờ...</p>
        </div>
    `;

    try {
        const url = `/Booking/GetSlots?staffId=${staffId}&serviceId=${serviceId}&date=${date}`;
        const res = await fetch(url, signal ? { signal } : {});
        
        if (!res.ok) {
            throw new Error("Server error: " + res.status);
        }
        
        const json = await res.json();

        if (!json?.success || !json.data?.length) {
            container.innerHTML = `
                <div class="slots-empty">
                    <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                        <circle cx="12" cy="12" r="10"/>
                        <path d="M8 15s1.5-2 4-2 4 2 4 2"/>
                        <line x1="9" y1="9" x2="9.01" y2="9"/>
                        <line x1="15" y1="9" x2="15.01" y2="9"/>
                    </svg>
                    <p>Không có khung giờ trống trong ngày này</p>
                    <small>Vui lòng chọn ngày khác hoặc stylist khác</small>
                </div>
            `;
            return;
        }

        // Count available slots
        const availableCount = json.data.filter(s => s.isAvail).length;
        
        container.innerHTML = `
            <div class="slots-header">
                <span class="slots-count">${availableCount} khung giờ trống</span>
            </div>
            <div class="slot-grid mt-3">
                ${json.data.map(s => `
                    <button type="button" 
                            class="slot-btn ${s.isAvail ? '' : 'unavailable'}" 
                            data-iso="${s.time}" 
                            ${s.isAvail ? '' : 'disabled'}
                            title="${s.isAvail ? 'Có thể đặt' : 'Đã được đặt'}">
                        ${s.label}
                        ${s.isAvail ? '' : '<small>Đã đặt</small>'}
                    </button>`).join("")}
            </div>
        `;

        initSlotSelection();

        // Auto-select first available slot
        const firstAvailable = container.querySelector(".slot-btn:not(:disabled)");
        if (firstAvailable) {
            firstAvailable.click();
        }

    } catch (e) {
        console.error("Error loading slots:", e);
        container.innerHTML = `
            <div class="slots-error">
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                    <circle cx="12" cy="12" r="10"/>
                    <line x1="12" y1="8" x2="12" y2="12"/>
                    <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>
                <p>Không thể tải giờ. Vui lòng thử lại.</p>
                <small>Khi thử lại, vui lòng chọn lại stylist</small>
            </div>
        `;
    }
}

/* ── Date Picker Change ──────────────────────────────────────── */
const datePicker = $("#datePicker");
if (datePicker) {
    // Set minimum date to today
    const today = new Date();
    const todayStr = today.toISOString().split("T")[0];
    datePicker.min = todayStr;
    
    // Set maximum date to 60 days from now
    const maxDate = new Date();
    maxDate.setDate(maxDate.getDate() + 60);
    datePicker.max = maxDate.toISOString().split("T")[0];

    // Update hint text
    const dateHint = $("#dateHint");

    datePicker.addEventListener("change", async function () {
        // Update hint text
        if (dateHint) {
            const selectedDate = new Date(this.value);
            const todayDate = new Date(todayStr);
            const diffTime = selectedDate - todayDate;
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

            if (diffDays === 0) {
                dateHint.textContent = "Hôm nay - Bạn có thể đặt lịch hôm nay";
                dateHint.style.color = "var(--c-accent)";
            } else if (diffDays === 1) {
                dateHint.textContent = "Ngày mai - Còn " + diffDays + " ngày nữa";
                dateHint.style.color = "var(--c-text-muted)";
            } else {
                dateHint.textContent = "Còn " + diffDays + " ngày nữa - Sẵn sàng đặt lịch!";
                dateHint.style.color = "var(--c-accent)";
            }
        }

        // Reload slots
        const staffId = $(".staff-card.selected")?.dataset.staffId;
        const serviceId = $("#hdnServiceId")?.value;
        if (staffId && serviceId) {
            if (_currentSlotAbort) _currentSlotAbort.abort();
            const ctrl = new AbortController();
            _currentSlotAbort = ctrl;
            await loadSlots(staffId, serviceId, this.value, ctrl.signal);
        }
    });
}

/* ── Admin Status Update ─────────────────────────────────────── */
$$(".status-form").forEach(form => {
    form.addEventListener("submit", function (e) {
        const action = this.dataset.action || "thực hiện thao tác này";
        if (!confirm(`Bạn có chắc muốn ${action}?`)) {
            e.preventDefault();
        }
    });
});

/* ── Smooth Scroll for Anchor Links ───────────────────────────── */
$$("a[href^='#']").forEach(anchor => {
    anchor.addEventListener("click", function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute("href"));
        if (target) {
            target.scrollIntoView({ behavior: "smooth", block: "start" });
        }
    });
});

/* ── Intersection Observer for Animations ─────────────────────── */
const observerOptions = {
    root: null,
    rootMargin: "0px",
    threshold: 0.1
};

const animateOnScroll = (entries, observer) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add("visible");
            observer.unobserve(entry.target);
        }
    });
};

const scrollObserver = new IntersectionObserver(animateOnScroll, observerOptions);

$$(".fade-up, .fade-in, .slide-in, .scale-in").forEach(el => {
    el.style.opacity = "0";
    scrollObserver.observe(el);
});

/* ── Parallax Effect ─────────────────────────────────────────── */
window.addEventListener("scroll", () => {
    const scrolled = window.pageYOffset;
    const hero = $(".hero-section");
    if (hero && scrolled < window.innerHeight) {
        hero.style.backgroundPositionY = scrolled * 0.4 + "px";
    }
});

/* ── Tab Filtering (MyBookings) ──────────────────────────────── */
$$(".tab-btn").forEach(btn => {
    btn.addEventListener("click", function () {
        $$(".tab-btn").forEach(b => b.classList.remove("active"));
        this.classList.add("active");

        const filter = this.dataset.filter;
        $$(".booking-card").forEach(card => {
            card.style.display = (filter === "all" || card.dataset.status === filter) ? "block" : "none";
        });
    });
});

/* ── Form Validation Enhancement ────────────────────────────── */
$$(".salon-form-control").forEach(input => {
    input.addEventListener("blur", function () {
        if (this.value && !this.classList.contains("is-invalid")) {
            this.classList.add("is-valid");
        }
    });

    input.addEventListener("input", function () {
        this.classList.remove("is-valid", "is-invalid");
    });
});

/* ── Confirmation Modal ─────────────────────────────────────── */
function showConfirmModal(message, onConfirm) {
    const overlay = document.createElement("div");
    overlay.className = "confirm-modal-overlay";
    const modalId = "cm-" + Date.now();
    overlay.innerHTML = `
        <div class="confirm-modal">
            <div class="confirm-modal-icon">
                <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <circle cx="12" cy="12" r="10"/>
                    <line x1="12" y1="8" x2="12" y2="12"/>
                    <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>
            </div>
            <h3>Xác nhận</h3>
            <p>${message}</p>
            <div class="confirm-modal-actions">
                <button class="btn btn-ghost js-modal-cancel">Hủy</button>
                <button class="btn btn-primary js-modal-confirm">Xác nhận</button>
            </div>
        </div>
    `;

    document.body.appendChild(overlay);

    overlay.querySelector(".js-modal-cancel").addEventListener("click", () => {
        document.body.removeChild(overlay);
    });

    overlay.querySelector(".js-modal-confirm").addEventListener("click", () => {
        document.body.removeChild(overlay);
        if (onConfirm) onConfirm();
    });

    overlay.addEventListener("click", (e) => {
        if (e.target === overlay) {
            document.body.removeChild(overlay);
        }
    });
}

/* ── Toast Notification ──────────────────────────────────────── */
function showToast(message, type = "info") {
    const toast = document.createElement("div");
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `
        <div class="toast-icon">
            ${type === "success" ? `
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M22 11.08V12a10 10 0 11-5.93-9.14"/>
                    <polyline points="22,4 12,14.01 9,11.01"/>
                </svg>` : `
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <circle cx="12" cy="12" r="10"/>
                    <line x1="12" y1="8" x2="12" y2="12"/>
                    <line x1="12" y1="16" x2="12.01" y2="16"/>
                </svg>`}
        </div>
        <span>${message}</span>
    `;

    document.body.appendChild(toast);

    setTimeout(() => toast.classList.add("show"), 10);
    setTimeout(() => {
        toast.classList.remove("show");
        setTimeout(() => toast.remove(), 300);
    }, 4000);
}

/* ── Loading Overlay ─────────────────────────────────────────── */
function showLoading() {
    const overlay = document.createElement("div");
    overlay.className = "loading-overlay";
    overlay.id = "loadingOverlay";
    overlay.innerHTML = `
        <div class="loading-spinner-large">
            <div class="spinner-ring"></div>
        </div>
    `;
    document.body.appendChild(overlay);
}

function hideLoading() {
    const overlay = $("#loadingOverlay");
    if (overlay) {
        overlay.classList.add("fade-out");
        setTimeout(() => overlay.remove(), 300);
    }
}

/* ── Init on DOM Ready ──────────────────────────────────────── */
document.addEventListener("DOMContentLoaded", () => {
    initStaffSelection();
    initSlotSelection();

    // Add visible class for animations
    document.querySelectorAll(".fade-up, .fade-in, .slide-in, .scale-in").forEach(el => {
        setTimeout(() => el.classList.add("visible"), 100);
    });

    // Back to top button
    const backToTop = $("#backToTop");
    if (backToTop) {
        window.addEventListener("scroll", () => {
            if (window.scrollY > 300) {
                backToTop.classList.add("visible");
            } else {
                backToTop.classList.remove("visible");
            }
        });

        backToTop.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        });
    }
});

/* ── Export Functions ────────────────────────────────────────── */
window.HairSalon = {
    showToast,
    showConfirmModal,
    showLoading,
    hideLoading,
    loadSlots
};
