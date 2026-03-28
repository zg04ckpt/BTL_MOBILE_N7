
export function toCamelCase(obj: any): any {
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }

    if (Array.isArray(obj)) {
        return obj.map((item) => toCamelCase(item));
    }

    const newObj: any = {};
    for (const key in obj) {
        if (Object.prototype.hasOwnProperty.call(obj, key)) {
            // Lấy key mới: chuyển chữ cái đầu từ in hoa sang in thường
            const camelKey = key.charAt(0).toLowerCase() + key.slice(1);
            newObj[camelKey] = toCamelCase(obj[key]);
        }
    }
    return newObj;
}

export const getStatusColor = (status: string): string => {
    if (status == 'Hoạt động') return 'success';
    if (status == 'Ngừng hoạt động') return 'default';
    if (status == 'Đang khoá') return 'error';
    if (status == 'Chờ duyệt') return 'warning';
    return 'processing';
}

export const convertToFormData = (data: any): FormData => {
    const formData = new FormData();

    Object.keys(data).forEach(key => {
        const value = data[key];
        if (value !== null && value !== undefined) { 
            if (Array.isArray(value) && value.length > 0 && value[0] instanceof File) {
                value.forEach((file: File) => {
                    formData.append(key, file);
                });
            } else if (Array.isArray(value)) {
                value.forEach((e, i) => {
                    addPropToFormData(formData, `${key}[${i}]`, e);
                });
            } else {
                addPropToFormData(formData, key, value);
            }
        }
    });

    return formData;
}

const addPropToFormData = (formData: FormData, key: string, value: any) => {
    if (value instanceof File) {
        formData.append(key, value);
    } else if (value instanceof Date) {
        formData.append(key, value.toISOString());
    } else if (typeof value === 'object') {
        addObjectToFormData(formData, key, value);
    } else {
        formData.append(key, value.toString());
    }
}

const addObjectToFormData = (formData: FormData, key: string, data: any) => {
    Object.keys(data).forEach(childKey => {
        const value = data[childKey];
        if(value) {
            if (value instanceof Array) {
                value.forEach((e, i)  => {
                    addPropToFormData(formData, `${key}.${childKey}`, e);
                });
            } else {
                addPropToFormData(formData, `${key}.${childKey}`, value);
            }
        }
    });
}

export function toQueryParams(obj: Record<string, any>, prefix = ''): string {
    const query = new URLSearchParams();

    const add = (key: string, value: any) => {
        if (value === null || value === undefined) return;
        if (value instanceof Date) {
            query.append(key, value.toISOString());
            return;
        }
        if (Array.isArray(value)) {
            value.forEach(v => add(key, v)); // flatten array
        } else if (typeof value === 'object') {
            // nếu muốn support nested object
            Object.keys(value).forEach(subKey => {
                add(`${key}[${subKey}]`, value[subKey]);
            });
        } else {
            query.append(key, value.toString());
        }
    };

    Object.keys(obj).forEach(key => {
        const value = obj[key];
        add(prefix ? `${prefix}[${key}]` : key, value);
    });

    return query.toString();
}