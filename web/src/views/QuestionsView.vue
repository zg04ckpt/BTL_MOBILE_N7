<script lang="ts" setup>
    import { getAllQuestions, getAllTopic, getQuestionDetail, updateQuestion, deleteQuestion, createQuestion, deleteQuestions } from '@/api';
    import * as XLSX from 'xlsx';
    import { QuestionLevel, QuestionListItemDto, QuestionStatus, SearchQuestionRequest, TopicListItemDto, UpdateQuestionRequest, QuestionDetailDto, QuestionLevelLabel, QuestionTypeLabel, QuestionStatusLabel } from '@/types';
    import { UploadOutlined, DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
    import { message, Modal } from 'ant-design-vue';
    import { computed, h, onMounted, reactive, ref } from 'vue';

    const selectedRowKeys = ref<number[]>([]);
    const isSelectRowStatus = ref<boolean>(false);
    const params = reactive<SearchQuestionRequest>({
        pageIndex: 1,
        pageSize: 5,
        level: null,
        isAsc: true,
        orderBy: 'Id',
        status: null,
        stringContent: '',
        topicId: null,
        type: null
    });
    const questions = ref<QuestionListItemDto[]>([]);
    const topics = ref<TopicListItemDto[]>([]);

    // Modal states
    const isUploadModalOpen = ref(false);
    const isEditModalOpen = ref(false);
    const isEditLoading = ref(false);
    const currentEditingQuestionId = ref<number | null>(null);
    const excelFile = ref<File | null>(null);

    // Form data for edit modal
    const editFormData = reactive<any>({
        stringContent: '',
        type: null,
        level: null,
        status: null,
        topicId: null,
        correctAnswers: [],
        stringAnswers: [],
        image: null,
        imageUrl: '',
        imagePreview: '',
        audio: null,
        audioUrl: '',
        audioPreview: '',
        video: null,
        videoUrl: '',
        videoPreview: ''
    });

    const pagination = reactive({
        pageSize: params.pageSize,
        current: params.pageIndex,
        showSizeChanger: true,
        total: 0
    });

    const columns = [
        {
            title: 'ID',
            dataIndex: 'id',
            key: 'id',
            width: 50
        },
        {
            title: 'Nội dung câu hỏi',
            dataIndex: 'stringContent',
            key: 'stringContent',
            width: 500
        },
        {
            title: 'Chủ đề',
            dataIndex: 'topicName',
            key: 'topicName',
            width: 180
        },
        {
            title: 'Level',
            dataIndex: 'level',
            key: 'level',
            width: 180
        },
        {
            title: 'Kiểu câu hỏi',
            dataIndex: 'type',
            key: 'type',
            width: 200
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            width: 180,
        },
        {
            title: 'Hành động',
            dataIndex: 'action',
            key: 'action',
            width: 100,
            fixed: 'right',
            customRender: ({record}: any) => h('div', {class: 'd-flex gap-2 justify-content-center'}, [
                h(EditOutlined, {
                    class: 'cursor-pointer text-primary',
                    style: {fontSize: '16px'},
                    onClick: () => showEditModal(record)
                }),
                h(DeleteOutlined, {
                    class: 'cursor-pointer text-danger',
                    style: {fontSize: '16px'},
                    onClick: () => showDeleteConfirm(record)
                })
            ])
        },
    ];

    const rowSelection = computed(() => ({
        selectedRowKeys: selectedRowKeys.value,
        onChange: (keys: number[]) => {
            selectedRowKeys.value = keys
            if (keys.length > 0) {
                isSelectRowStatus.value = true;
            } else {
                isSelectRowStatus.value = false;
            }
        }
    }));

    const handleSearchQuestion = async () => {
        const hideLoading = message.loading("Đang xử lý ...", 0);
        const res = await getAllQuestions(params);
        hideLoading();

        if (res.isSuccess) {
            questions.value = res.data!.items;
            pagination.total = res.data!.totalItems;
            pagination.current = res.data!.pageIndex;
            pagination.pageSize = res.data!.pageSize;
        } else {
            message.error("Tải dữ liệu thất bại: " + res.message);
        }
    }

    const handleLoadTopics = async () => {
        const res = await getAllTopic();
        if (res.isSuccess) {
            topics.value = res.data!;
        }
    }

    const handleOnPaginationChange = (pagination: any) => {
        params.pageIndex = pagination.current;
        params.pageSize = pagination.pageSize;
        handleSearchQuestion();
    }

    // Upload Excel handlers
    const showUploadModal = () => {
        isUploadModalOpen.value = true;
        excelFile.value = null;
    };

    const handleExcelUpload = null; // kept for compatibility

    const handleBeforeUpload = (file: File) => {
        parseExcelAndCreate(file);
        return false; // prevent a-upload from auto uploading
    };

    const parseExcelAndCreate = async (file: File) => {
        try {
            const arrayBuffer = await file.arrayBuffer();
            const workbook = XLSX.read(arrayBuffer, { type: 'array' });
            const sheet = workbook.Sheets[workbook.SheetNames[0]];
            const rows: any[] = XLSX.utils.sheet_to_json(sheet, { header: 1, defval: '' });

            if (!rows || rows.length === 0) {
                message.error('File Excel rỗng');
                return;
            }

            // Detect header row (if first cell contains keywords)
            let startRow = 0;
            const firstRow = rows[0];
            if (firstRow && typeof firstRow[0] === 'string') {
                const cell = firstRow[0].toLowerCase();
                if (cell.includes('nội dung') || cell.includes('content') || cell.includes('câu')) {
                    startRow = 1;
                }
            }

            const createRequests: any[] = [];
            for (let i = startRow; i < rows.length; i++) {
                const row = rows[i];
                if (!row) continue;
                const stringContent = String(row[0] || '').trim();
                if (!stringContent) continue;
                const topicId = row[1] ? Number(row[1]) : null;
                const level = row[2] !== undefined && row[2] !== '' ? Number(row[2]) : 0;
                const type = row[3] !== undefined && row[3] !== '' ? Number(row[3]) : 0;
                const correctAnswers = String(row[4] || '').split(';').map((s: string) => s.trim()).filter(Boolean);
                const stringAnswers = String(row[5] || '').split(';').map((s: string) => s.trim()).filter(Boolean);

                // topicId is required by API - skip rows without topicId
                if (!topicId) continue;

                createRequests.push({
                    stringContent,
                    topicId,
                    level,
                    type,
                    correctAnswers,
                    stringAnswers
                });
            }

            if (createRequests.length === 0) {
                message.error('Không tìm thấy dòng hợp lệ trong file Excel');
                return;
            }

            // Send sequential create requests with progress
            let hideLoading = message.loading(`Đang tạo 0/${createRequests.length}`, 0);
            const errors: Array<{ index: number; message: string }> = [];
            let successCount = 0;
            for (let i = 0; i < createRequests.length; i++) {
                hideLoading();
                hideLoading = message.loading(`Đang tạo ${i + 1}/${createRequests.length}`, 0);
                const req = createRequests[i];
                try {
                    const res = await createQuestion(req as any);
                    if (res.isSuccess) {
                        successCount++;
                    } else {
                        errors.push({ index: i + 1 + startRow, message: res.message || 'Lỗi' });
                    }
                } catch (err: any) {
                    errors.push({ index: i + 1 + startRow, message: err?.message || 'Lỗi khi gọi API' });
                }
            }
            hideLoading();

            if (errors.length === 0) {
                message.success(`Nhập thành công ${successCount}/${createRequests.length} câu hỏi`);
            } else {
                message.error(`Hoàn thành: ${successCount}/${createRequests.length}. Lỗi ở ${errors.length} dòng.`);
                console.error('Import errors', errors);
            }

            isUploadModalOpen.value = false;
            handleSearchQuestion();
        } catch (error) {
            message.error('Lỗi khi xử lý file Excel');
            console.error(error);
        }
    };

    const handleFileChange = (payload: any, type: 'image' | 'audio' | 'video') => {
        // payload can be: native Event, File, or Ant Upload file object (with originFileObj)
        let file: File | null = null;
        if (!payload) return;

        if ((payload as any).target && (payload as any).target.files) {
            const files = (payload as any).target.files as FileList;
            if (files && files.length > 0) file = files[0];
        } else if ((payload as any).originFileObj) {
            file = (payload as any).originFileObj as File;
        } else if (payload instanceof File) {
            file = payload as File;
        }

        if (!file) return;

        (editFormData as any)[type] = file;
        const reader = new FileReader();
        reader.onload = (ev: ProgressEvent<FileReader>) => {
            (editFormData as any)[type + 'Preview'] = ev.target?.result as string;
        };
        reader.readAsDataURL(file);
    };

    const clearMedia = (type: 'image' | 'audio' | 'video') => {
        (editFormData as any)[type] = null;
        (editFormData as any)[type + 'Preview'] = '';
        (editFormData as any)[type + 'Url'] = '';
    };

    // Edit Question handlers
    const showEditModal = async (record: QuestionListItemDto) => {
        isEditLoading.value = true;
        try {
            const res = await getQuestionDetail(record.id);
            if (res.isSuccess && res.data) {
                const detail = res.data as QuestionDetailDto;
                editFormData.stringContent = detail.stringContent;
                editFormData.type = detail.type;
                editFormData.level = detail.level;
                editFormData.status = detail.status;
                editFormData.topicId = detail.topicId;
                editFormData.correctAnswers = detail.correctAnswers;
                editFormData.stringAnswers = detail.stringAnswers;
                // media urls
                editFormData.imageUrl = detail.imageUrl || '';
                editFormData.imagePreview = '';
                editFormData.image = null;
                editFormData.audioUrl = (detail as any).audioUrl || '';
                editFormData.audioPreview = '';
                editFormData.audio = null;
                editFormData.videoUrl = (detail as any).videoUrl || '';
                editFormData.videoPreview = '';
                editFormData.video = null;
                currentEditingQuestionId.value = detail.id;
                isEditModalOpen.value = true;
            } else {
                message.error('Lấy dữ liệu câu hỏi thất bại');
            }
        } catch (error) {
            message.error('Lỗi khi tải dữ liệu câu hỏi');
        } finally {
            isEditLoading.value = false;
        }
    };

    const handleEditSubmit = async () => {
        if (!editFormData.stringContent || !editFormData.topicId || editFormData.type === null || editFormData.level === null) {
            message.error('Vui lòng điền đầy đủ các trường bắt buộc');
            return;
        }

        if (editFormData.correctAnswers.length === 0 || editFormData.stringAnswers.length === 0) {
            message.error('Vui lòng nhập câu trả lời');
            return;
        }

        isEditLoading.value = true;
        const hideLoading = message.loading("Đang cập nhật ...", 0);
        try {
            const updateRequest: any = {
                stringContent: editFormData.stringContent,
                type: editFormData.type,
                level: editFormData.level,
                status: editFormData.status,
                topicId: editFormData.topicId,
                correctAnswers: editFormData.correctAnswers,
                stringAnswers: editFormData.stringAnswers
            };
            if (editFormData.image) updateRequest.image = editFormData.image;
            if (editFormData.audio) updateRequest.audio = editFormData.audio;
            if (editFormData.video) updateRequest.video = editFormData.video;

            const res = await updateQuestion(currentEditingQuestionId.value!, updateRequest);
            hideLoading();
            
            if (res.isSuccess) {
                message.success('Cập nhật câu hỏi thành công');
                isEditModalOpen.value = false;
                handleSearchQuestion();
            } else {
                message.error('Cập nhật câu hỏi thất bại: ' + res.message);
            }
        } catch (error) {
            message.error('Lỗi khi cập nhật câu hỏi');
        } finally {
            isEditLoading.value = false;
        }
    };


    // Delete Question handlers
    const showDeleteConfirm = (record: QuestionListItemDto) => {
        Modal.confirm({
            title: 'Xác nhận xóa',
            content: `Bạn có chắc chắn muốn xóa câu hỏi: "${record.stringContent.substring(0, 50)}..."?`,
            okText: 'Xóa',
            okType: 'danger',
            cancelText: 'Hủy',
            onOk() {
                handleDeleteQuestion(record.id);
            }
        });
    };

    const handleDeleteQuestion = async (questionId: number) => {
        const hideLoading = message.loading('Đang xóa...', 0);
        try {
            const res = await deleteQuestion(questionId);
            hideLoading();
            if (res.isSuccess) {
                message.success('Xóa câu hỏi thành công');
                handleSearchQuestion();
            } else {
                message.error('Xóa câu hỏi thất bại: ' + res.message);
            }
        } catch (error) {
            hideLoading();
            message.error('Lỗi khi xóa câu hỏi');
        }
    };

    // Xóa nhiều câu hỏi
    const handleDeleteSelectedQuestions = async () => {
        if (selectedRowKeys.value.length === 0) return;
        Modal.confirm({
            title: 'Xác nhận xóa',
            content: `Bạn có chắc chắn muốn xóa ${selectedRowKeys.value.length} câu hỏi đã chọn?`,
            okText: 'Xóa',
            okType: 'danger',
            cancelText: 'Hủy',
            async onOk() {
                const hideLoading = message.loading('Đang xóa...', 0);
                try {
                    const res = await deleteQuestions(selectedRowKeys.value);
                    hideLoading();
                    if (res.isSuccess) {
                        message.success('Đã xóa các câu hỏi đã chọn');
                        selectedRowKeys.value = [];
                        isSelectRowStatus.value = false;
                        handleSearchQuestion();
                    } else {
                        message.error('Xóa thất bại: ' + res.message);
                    }
                } catch (error) {
                    hideLoading();
                    message.error('Lỗi khi xóa nhiều câu hỏi');
                }
            }
        });
    };

    onMounted(() => {
        handleLoadTopics();
        handleSearchQuestion();
    });
</script>

<template >
    <!-- Title + Action -->
    <div class="d-flex align-items-center mb-3">
        <h2>Quản lý câu hỏi</h2>
        <div class="flex-fill"></div>
        <a-button type="primary" @click="showUploadModal">
            <UploadOutlined />
            Thêm câu hỏi bằng Excel
        </a-button>
    </div>

    <!-- Search + Filter -->
    <div v-if="!isSelectRowStatus" class="d-flex align-items-center mb-3" style="height: 40px;">
        <a-input-search
            class="w-25"
            v-model:value="params.stringContent"
            placeholder="Nhập nội dung câu hỏi"
            loading:value="false"
            enter-button
            @search="handleSearchQuestion"/>
        
        <div class="flex-fill"></div>
        <a-select
            class="me-2"
            ref="select"
            v-model:value="params.topicId"
            style="width: 120px"
            @focus="() => {}"
            @change="(value:any) => {
                params.topicId = value;
                handleSearchQuestion();
            }">
            <a-select-option :value="null" selected>Mọi chủ đề</a-select-option>
            <a-select-option v-for="t in topics" :value="t.id">{{ t.name }}</a-select-option>
        </a-select>
        <a-select
            class="me-2"
            ref="select"
            v-model:value="params.level"
            style="width: 120px"
            @focus="() => {}"
            @change="(value:any) => {
                params.level = value;
                handleSearchQuestion();
            }">
            <a-select-option :value="null">Mọi cấp độ</a-select-option>
            <a-select-option :value="QuestionLevel.Normal">Dễ</a-select-option>
            <a-select-option :value="QuestionLevel.Medium">Trung bình</a-select-option>
            <a-select-option :value="QuestionLevel.Hard">Khó</a-select-option>
        </a-select>
        <a-select
            class=""
            ref="select"
            v-model:value="params.status"
            style="width: 140px"
            @focus="() => {}"
            @change="(value:any) => {
                params.status = value;
                handleSearchQuestion();
            }">
            <a-select-option :value="null">Mọi trạng thái</a-select-option>
            <a-select-option :value="QuestionStatus.Draft">Đang chỉnh sửa</a-select-option>
            <a-select-option :value="QuestionStatus.Pending">Đang đợi duyệt</a-select-option>
            <a-select-option :value="QuestionStatus.Approved">Đã duyệt</a-select-option>
            <a-select-option :value="QuestionStatus.Rejected">Đã từ chối</a-select-option>
        </a-select>
    </div>

    <!-- Action for selected rows -->
    <div v-if="isSelectRowStatus" class="d-flex align-items-center mb-3" style="height: 40px;">
        <div class="align-content-center fw-bold">Đã chọn {{ selectedRowKeys.length }}</div>
        <a-button type="primary" class="ms-3" @click="handleDeleteSelectedQuestions">
            <DeleteOutlined />
            Xóa
        </a-button>
    </div>

    <!-- Table data -->
    <div class="card card-body p-2">
        <a-table 
            rowKey="id"
            size="small" 
            :row-selection="rowSelection" 
            :columns="columns" 
            :scroll="{ x: 'max-content' }"
            :pagination="pagination"
            :data-source="questions"
            @change="handleOnPaginationChange" />
    </div>

    <!-- Upload Excel Modal -->
    <a-modal
        v-model:open="isUploadModalOpen"
        title="Nhập câu hỏi từ file Excel"
        @ok="() => isUploadModalOpen = false"
        ok-text="Đóng"
        :mask-closable="false"
    >
        <a-upload
            accept=".xlsx,.xls"
            list-type="text"
            :max-count="1"
            :before-upload="handleBeforeUpload"
            :show-upload-list="false"
        >
            <template #default>
                <a-button>
                    <UploadOutlined />
                    Chọn file Excel
                </a-button>
            </template>
        </a-upload>
        <div class="mt-3">
            <p><strong>Cấu trúc file Excel:</strong></p>
            <ul>
                <li>Cột A: Nội dung câu hỏi (bắt buộc)</li>
                <li>Cột B: Chủ đề ID (bắt buộc)</li>
                <li>Cột C: Level (0=Normal, 1=Medium, 2=Hard)</li>
                <li>Cột D: Type (0=SingleChoice, 1=MultipleChoice, 2=TrueFalse)</li>
                <li>Cột E: Câu trả lời đúng (phân tách bằng ;)</li>
                <li>Cột F: Câu trả lời (phân tách bằng ;)</li>
            </ul>
        </div>
    </a-modal>

    <!-- Edit Question Modal -->
    <a-modal
        v-model:open="isEditModalOpen"
        title="Cập nhật câu hỏi"
        ok-text="Lưu"
        cancel-text="Hủy"
        :confirm-loading="isEditLoading"
        @ok="handleEditSubmit"
        width="700px"
    >
        <a-form layout="vertical">
            <a-form-item label="Nội dung câu hỏi" required>
                <a-textarea
                    v-model:value="editFormData.stringContent"
                    placeholder="Nhập nội dung câu hỏi"
                    :rows="4"
                />
            </a-form-item>

            <div class="d-flex gap-3">
                <a-form-item label="Chủ đề" required class="flex-fill">
                    <a-select
                        v-model:value="editFormData.topicId"
                        placeholder="Chọn chủ đề"
                    >
                        <a-select-option v-for="t in topics" :key="t.id" :value="t.id">
                            {{ t.name }}
                        </a-select-option>
                    </a-select>
                </a-form-item>

                <a-form-item label="Level" required class="flex-fill">
                    <a-select
                        v-model:value="editFormData.level"
                        placeholder="Chọn level"
                    >
                        <a-select-option :value="0">Dễ (Normal)</a-select-option>
                        <a-select-option :value="1">Trung bình (Medium)</a-select-option>
                        <a-select-option :value="2">Khó (Hard)</a-select-option>
                    </a-select>
                </a-form-item>
            </div>

            <div class="d-flex gap-3">
                <a-form-item label="Kiểu câu hỏi" required class="flex-fill">
                    <a-select
                        v-model:value="editFormData.type"
                        placeholder="Chọn kiểu"
                    >
                        <a-select-option :value="0">Single Choice</a-select-option>
                        <a-select-option :value="1">Multiple Choice</a-select-option>
                        <a-select-option :value="2">True False</a-select-option>
                    </a-select>
                </a-form-item>

                <a-form-item label="Trạng thái" class="flex-fill">
                    <a-select
                        v-model:value="editFormData.status"
                        placeholder="Chọn trạng thái"
                    >
                        <a-select-option :value="0">Đang chỉnh sửa</a-select-option>
                        <a-select-option :value="1">Đang đợi duyệt</a-select-option>
                        <a-select-option :value="2">Đã duyệt</a-select-option>
                        <a-select-option :value="3">Đã từ chối</a-select-option>
                    </a-select>
                </a-form-item>
            </div>

            <a-form-item label="Câu trả lời đúng" required>
                <a-select
                    v-model:value="editFormData.correctAnswers"
                    mode="tags"
                    placeholder="Nhập câu trả lời đúng (nhấn Enter để thêm)"
                />
            </a-form-item>

            <a-form-item label="Các câu trả lời" required>
                <a-select
                    v-model:value="editFormData.stringAnswers"
                    mode="tags"
                    placeholder="Nhập các câu trả lời (nhấn Enter để thêm)"
                />
            </a-form-item>

            <a-form-item label="Ảnh minh họa">
                <div class="d-flex flex-column">
                    <a-upload
                        accept="image/*"
                        :before-upload="() => false"
                        :show-upload-list="false"
                        @change="(info: any) => handleFileChange(info.file || info, 'image')"
                    >
                        <a-button>
                            <UploadOutlined /> Chọn ảnh
                        </a-button>
                    </a-upload>
                    <div class="mt-2 d-flex flex-column align-items-start">
                        <a-image v-if="editFormData.imagePreview || editFormData.imageUrl" :src="editFormData.imagePreview || editFormData.imageUrl" width="120" />
                        <a-button v-if="editFormData.imagePreview || editFormData.imageUrl" type="link" @click="() => clearMedia('image')">Xóa</a-button>
                    </div>
                </div>
            </a-form-item>

            <a-form-item label="Audio">
                <div class="d-flex flex-column w-100">
                    <a-upload
                        accept="audio/*"
                        :before-upload="() => false"
                        :show-upload-list="false"
                        @change="(info: any) => handleFileChange(info.file || info, 'audio')"
                    >
                        <a-button>
                            <UploadOutlined /> Chọn audio
                        </a-button>
                    </a-upload>
                    <div class="mt-2 d-flex flex-column align-items-start">
                        <audio v-if="editFormData.audioPreview || editFormData.audioUrl" controls :src="editFormData.audioPreview || editFormData.audioUrl" style="max-width: 100%;"></audio>
                        <a-button v-if="editFormData.audioPreview || editFormData.audioUrl" type="link" @click="() => clearMedia('audio')">Xóa</a-button>
                    </div>
                </div>
            </a-form-item>

            <a-form-item label="Video">
                <div class="d-flex flex-column">
                    <a-upload
                        accept="video/*"
                        :before-upload="() => false"
                        :show-upload-list="false"
                        @change="(info: any) => handleFileChange(info.file || info, 'video')"
                    >
                        <a-button>
                            <UploadOutlined /> Chọn video
                        </a-button>
                    </a-upload>
                    <div class="mt-2 d-flex flex-column align-items-start">
                        <video v-if="editFormData.videoPreview || editFormData.videoUrl" controls :src="editFormData.videoPreview || editFormData.videoUrl" style="max-width: 100%; max-height: 200px;"></video>
                        <a-button v-if="editFormData.videoPreview || editFormData.videoUrl" type="link" @click="() => clearMedia('video')">Xóa</a-button>
                    </div>
                </div>
            </a-form-item>
        </a-form>
    </a-modal>
</template>

<style scoped>
.cursor-pointer {
    cursor: pointer;
    transition: opacity 0.2s;
}

.cursor-pointer:hover {
    opacity: 0.7;
}

.text-danger {
    color: #ff4d4f;
}
</style>
