export interface TaskItem{
    id: string;
    title: string;
    description: string;
    isCompleted: boolean;
    dueDate: string | null;
}