window.mealNotify = {
    async requestPermission() {
        if (!("Notification" in window)) {
            return false;
        }

        if (Notification.permission === "granted") {
            return true;
        }

        if (Notification.permission === "denied") {
            return false;
        }

        const permission = await Notification.requestPermission();
        return permission === "granted";
    },

    show(title, body) {
        if (!("Notification" in window) || Notification.permission !== "granted") {
            return false;
        }

        new Notification(title, {
            body,
            tag: "ai-meal-planner",
            renotify: true
        });
        return true;
    }
};
